using Cryptobot.AIClass;
using Cryptobot.Models.HelpModels;
using Cryptobot.Stocks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using TelerikWinFormsApp1.Models.HelpModels;

namespace Cryptobot
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        public List<Stock> Stock { get; set; } = new List<Stock>() {
                                                        new Bitbay(),
                                                        new Bittrex(),
                                                        new Coinex(),
                                                        new CryptoBridge(),
                                                        //new Exmo(),
                                                        new Gate(),
                                                        new Kucoin(),
                                                        new LiveCoin(),
                                                        new Nova(),
                                                        new Poloniex(),
                                                        //new Yobit()
                                                    };

        TaskScheduler context;
        CancellationToken token = Task.Factory.CancellationToken;
        string LastButtonPressed = "";
        public RadForm1()
        {

            InitializeComponent();
            context = TaskScheduler.FromCurrentSynchronizationContext();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }


        private void RadForm1_Load(object sender, EventArgs e)
        {
            StockWaitingBar.StartWaiting();
            var listitems = new List<ListViewDataItem>();
            var bw = new BackgroundWorker();
            bw.DoWork += (o, args) =>
            {

                foreach (var item in Stock)
                {
                    var temp = new ListViewDataItem(item.StockName);
                    if (!item.CheakStock())
                    {
                        temp.Enabled = false;
                    }
                    listitems.Add(temp);
                }
            };
            bw.RunWorkerCompleted += (o, args) =>
            {
                StocksCheakBox.Items.AddRange(listitems.ToArray());
                StockWaitingBar.Hide();
            };
            bw.RunWorkerAsync();

            var order = Stock.First(x => x.StockName == "Bittrex").Orders.GetOrderAsync("BTC", "USD");
            BTC_USD.Text = order.Result.asks.First().Key.ToString();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        public void Updatetimer(Object source, ElapsedEventArgs e)
        {
            var token = Task.Factory.CancellationToken;
            Task.Factory.StartNew(() =>
            {
                TimerView.Text = (Convert.ToInt32(TimerView.Text) + 1).ToString();
            }, token, TaskCreationOptions.None, context);
        }


        public List<TranzactionCoinModel> UpdateCoinsTranzactionAlg(List<Stock> stocks)
        {
            var equalsCoins = new CheakEqualsCurrencies(stocks, CheakNamesCheakBox.Checked, CheakWalletsCheakBox.Checked).GetSamePairs();
            var models = new TranzactionCoinCompare(equalsCoins).TranzactionCompare();
            return models;
        }

        public void UpdateListTranzactionAlg()
        {
            var stocks = GetActiveStocks();
            var models = UpdateCoinsTranzactionAlg(stocks);
            Task.Factory.StartNew(() =>
            {
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                dataGridView1.DataSource = new BindingList<TranzactionCoinViewModel>(models.Select(x => new TranzactionCoinViewModel()
                {
                    Pair = x.PairName,
                    MarketCoin = x.MarketCoin,
                    BaseCoin = x.BaseCoin,
                    FirstStock = x.FirstStock.StockName,
                    SecondStock = x.SecondStock.StockName,
                    BuyBid = x.BuyBid,
                    SellAks = x.SellAks,
                    Profit = x.Profit

                }).ToList());
                dataGridView1.AutoSizeRows = true;
                dataGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }, token, TaskCreationOptions.None, context);


            //decimal divided = item.StrategyBuy.Sum(x => x.Key * x.Value);
            //decimal divider = item.StrategyBuy.Sum(x => x.Value);
            //decimal AverBuyPrice = Math.Round(divided / divider, 8);

            //decimal divided2 = item.StrategySell.Sum(x => x.Key * x.Value);
            //decimal divider2 = item.StrategySell.Sum(x => x.Value);
            //decimal AverSellPrice = Math.Round(divided2 / divider2, 8);

            //decimal Quantity = item.StrategyBuy.Sum(d => d.Value);
            //decimal BlackProfit = Math.Round((AverSellPrice - AverBuyPrice) * Quantity, 8);
            //decimal FeeCost = Math.Round(((Quantity * AverBuyPrice) * StockFee), 8);
            //decimal Profit = (BlackProfit - FeeCost);

            //if (item.MarketName.Split('-')[0] == "USDT")
            //{
            //    Profit -= 15m;
            //}
            //else if (item.MarketName.Split('-')[0] == "USD")
            //{
            //    Profit -= ((AverBuyPrice * Quantity) * USDTransactionFee);
            //}


        }
        private void UpdateListArbitrationAlg()
        {
            var stocks = GetActiveStocks();
            var ViewModelList = UpdateCoinsArbitrationAlg(stocks);
            Task.Factory.StartNew(() =>
            {
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                dataGridView1.DataSource = new BindingList<ArbitrationViewModel>(ViewModelList);
                dataGridView1.AutoSizeRows = true;
                dataGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }, token, TaskCreationOptions.None, context);

        }
        private void UpdateListEqualsCoins()
        {
            var stocks = GetActiveStocks();
            var equalsCoins = new CheakEqualsCurrencies(stocks, CheakNamesCheakBox.Checked, CheakWalletsCheakBox.Checked).GetSamePairs();

            Task.Factory.StartNew(() =>
            {
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                dataGridView1.DataSource = new BindingList<EqualsCoinViewModel>(
                    equalsCoins.Select(x => new EqualsCoinViewModel()
                    {
                        Pair = x.Market,
                        MarketCoin = x.MarketCoin,
                        BaseCoin = x.BaseCoin,
                        SecondBaseCoin = x.secBaseCoin,
                        SecondMarketCoin = x.secMarketCoin,
                        FirstStock = x.FirstStock.StockName,
                        SecondStock = x.SecondStock.StockName,
                        Ask = x.FirstStockAsk.ToString() + "\n" + x.SecondStockAsk.ToString(),
                        Bid = x.FirstStockBid.ToString() + "\n" + x.SecondStockBid.ToString()
                    }).ToList()
                );
                dataGridView1.AutoSizeRows = true;
                dataGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }, token, TaskCreationOptions.None, context);

        }
        private List<ArbitrationViewModel> UpdateCoinsArbitrationAlg(List<Stock> stocks)
        {

            var Cover = new List<ArbitrationModel>();
            foreach (var item in stocks)
            {
                var temp = new ArbitrationByOneStock(item);
                Cover.AddRange(temp.models);
            }
            var list = new List<ArbitrationViewModel>();
            foreach (var item in Cover)
            {
                list.AddRange(ArbitrationCoinCompare.ArbitrationCompare(item, 1m));
            }
            return list;
        }
        private void Tranzaction_Click(object sender, EventArgs e)
        {
            LastButtonPressed = "Tranzaction";
            TimerView.Text = "0";
            Task h = Task.Factory.StartNew(() =>
            {
                System.Timers.Timer timer = new System.Timers.Timer(1000);
                timer.Elapsed += Updatetimer;
                timer.Start();
                UpdateListTranzactionAlg();
                timer.Stop();
            });
        }
        private void TripleArbitration_Click(object sender, EventArgs e)
        {
            LastButtonPressed = "Arbitration";
            TimerView.Text = "0";
            Task h = Task.Factory.StartNew(() =>
            {
                System.Timers.Timer a = new System.Timers.Timer(1000);
                a.Elapsed += Updatetimer;
                a.Start();
                UpdateListArbitrationAlg();
                a.Stop();
            });
        }
        private void EqualsButton_Click(object sender, EventArgs e)
        {
            LastButtonPressed = "Equals";
            TimerView.Text = "0";
            Task h = Task.Factory.StartNew(() =>
            {
                System.Timers.Timer a = new System.Timers.Timer(1000);
                a.Elapsed += Updatetimer;
                a.Start();
                UpdateListEqualsCoins();
                a.Stop();
            });
        }
        private List<Stock> GetActiveStocks()
        {
            var SelectStocks = StocksCheakBox.CheckedItems;
            List<Stock> ActiveStock = new List<Stock>();
            foreach (var item in SelectStocks)
            {
                ActiveStock.Add(Stock.First(x => x.StockName == item.Text));
            }
            return ActiveStock;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://international.bittrex.com/Market/Index?MarketName=USD-BTC");
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void radPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void SerializeButton_Click(object sender, EventArgs e)
        {
            string path = "";
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
            }
            if (LastButtonPressed != "")
            {
                var data = dataGridView1.DataSource;
                string output = JsonConvert.SerializeObject(data);
                System.IO.File.WriteAllText(path + "\\json.json", output);
                MessageBox.Show("Serialized well", "Serialized", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Nothing to serialize", "Serialized", MessageBoxButtons.OK);
            }
        }


    }
}
