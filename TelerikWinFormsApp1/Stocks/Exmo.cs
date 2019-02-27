using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Interfesse;
using Cryptobot.Models.InfoModels;
using Cryptobot.Models.OrdersModels;
using Cryptobot.Models.TickerModels;
using Cryptobot.Models.TraidPairsModels;

namespace Cryptobot.Stocks
{
    public class ExmoTiker 
    {
        public string site = "https://api.exmo.com/v1/ticker/";

        public BaseTickerModel GetTikers()
        {
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, ExmoTikerField>>(str);
                var model = new ExmoTikerModel();
                model.tiker = res;
                return model.ToBaseTikerModel();
            }
        }

        public async Task<BaseTickerModel> GetTikersAsync()
        {
            return await Task<BaseTickerModel>.Factory.StartNew(() => GetTikers());
        }
    }
    public class ExmoTraidPairs : IGetTraidPairs
    {
        Dictionary<string, string> IGetTraidPairs.ExeptionPairs { get; set; } = new Dictionary<string, string>();

        public string site = "https://api.exmo.com/v1/pair_settings/";
        public BaseTraidPairModel GetTraidPairs()
        {
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string,ExmoTraidPairField>>(str);
                var model = new ExmoTraidPairsModel();
                model.Pairs = res;
                return model.ToBaseTraidPairs();
            }
        }

        public async Task<BaseTraidPairModel> GetTraidPairsAsync()
        {
            return await Task<BaseTraidPairModel>.Factory.StartNew(() => GetTraidPairs());
        }
    }
    public class ExmoInfo : IGetInfo
    {
        string site = "https://api.exmo.com/v1/currency/";

        public BaseInfoModel GetInfo()
        {
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<ExmoInfoModel>(str);
                return res.ToBaseInfoModel();
            }

        }
        public async Task<BaseInfoModel> GetInfoAsync()
        {
            return await Task<BaseInfoModel>.Factory.StartNew(() => GetInfo());
        }
    }

    public class ExmoGetOrders : IGetOrders
    {
        string OrderApi = "	https://api.exmo.com/v1/order_book/?pair=";
        public BaseOrdersModel GetOrders(List<KeyValuePair<string, string>> arg)
        {
            string allPairs = "";
            for (int i = 0; i < arg.Count; i++)
            {
                if (i == arg.Count - 1)
                {
                    allPairs += arg.ElementAt(i).Key;
                }
                else
                {
                    allPairs += arg.ElementAt(i).Key + ',';
                }
            }
            string site = OrderApi + allPairs;
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                   resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, ExmoOrderField>>(str);
                return new ExmoOrdersModel() { Pair = res }.ToBaseOrderModel();
            }
        }
        public BaseOrderModel GetOrder(string MainCoinName, string SecondCoinName)
        {
            string site = OrderApi + MainCoinName + "_" + SecondCoinName;

            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                   resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, ExmoOrderField>>(str);
                return new ExmoOrderModel() { Pair = res }.ToBaseOrderModel();
            }
        }
        public BaseOrdersModel GetAllOrder()
        {
            var pairs = new ExmoTraidPairs().GetTraidPairs();
            string allPairs = "";
            for (int i = 0; i < pairs.Pairs.Count; i++)
            {
                if (i == pairs.Pairs.Count - 1)
                {
                    allPairs += pairs.Pairs.ElementAt(i).Key;
                }
                else
                {
                    allPairs += pairs.Pairs.ElementAt(i).Key + ',';
                }
            }
            string site = OrderApi + allPairs;

            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                   resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, ExmoOrderField>>(str);
                return new ExmoOrdersModel() { Pair = res }.ToBaseOrderModel();
            }
        }
        public async Task<BaseOrdersModel> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return await Task<BaseOrdersModel>.Factory.StartNew(() => GetOrders(arg));
        }

        public async Task<BaseOrderModel> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return await Task<BaseOrderModel>.Factory.StartNew(() => GetOrder(MainCoinName, SecondCoinName));
        }

        public async Task<BaseOrdersModel> GetAllOrdersAsync()
        {
            return await Task<BaseOrdersModel>.Factory.StartNew(() => GetAllOrder());
        }
    }

    public class ExmoWallet : IWallet
    {
        public string Balance = "user_info";
        public string Address = "deposit_address";
        public class ExmoBalanceField
        {
            public Dictionary<string, string> balances;
            public Dictionary<string, string> reserved;
        }
        public Dictionary<string, TransformBallans> GetBalances()
        {
            var postData = new Dictionary<string, string>();
            var str = ExmoPostRequst.PostString(Balance, postData);
            Dictionary<string, TransformBallans> temp = new Dictionary<string, TransformBallans>();
            var t = JsonConvert.DeserializeObject<ExmoBalanceField>(str);

            foreach (var item in t.balances)
            {
                var buf = new TransformBallans();
                buf.Available = Convert.ToDecimal(item.Value.Replace('.', ','));
                buf.OnOrders = Convert.ToDecimal(t.reserved.Where(x => x.Key == item.Key).
                                                            First().Value.Replace('.', ','));
                temp.Add(item.Key, buf);
            }
            return temp;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var postData = new Dictionary<string, string>();
            var str = ExmoPostRequst.PostString(Balance, postData);
            var t = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
            return t;
        }
        public Task<Dictionary<string, TransformBallans>> GetBalancesAsync()
        {
            return Task<Dictionary<string, TransformBallans>>.Factory.StartNew(() => GetBalances());
        }

        public Task<Dictionary<string, string>> GetDepositAddressesAsync()
        {
            return Task<Dictionary<string, string>>.Factory.StartNew(() => GetDepositAddresses());
        }




    }
    public class ExmoTraid : ITrading
    {
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            throw new NotImplementedException();
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote)
        {
            throw new NotImplementedException();
        }
    }
    public class Exmo : Stock
    {
        public Exmo()
        {
            HasCoinsName = false;
            HasWalletStatus = false;
            StockName = "Exmo";
            Info = new ExmoInfo();
            Orders = new ExmoGetOrders();
            Ballans = new ExmoWallet();
            Traid = new ExmoTraid();
            TraidPairs = new ExmoTraidPairs();
        }
    }

    
}
