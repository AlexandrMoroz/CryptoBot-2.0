using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class GateTiker 
    {
        string site = "https://data.gateio.co/api2/1/tickers";

        public BaseTickerModel GetTikers()
        {
            WebResponse resp = GetRequst.Requst(site);
            List<string> temp = new List<string>();
            using (StreamReader stream = new StreamReader
                     (resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var a = JsonConvert.DeserializeObject<Dictionary<string, GetTikersField>>(str);
                return  new GetTikersModel(a).ToBaseTikerModel();
            }
        }

        public async Task<BaseTickerModel> GetTikersAsync()
        {
            return await Task<BaseTickerModel>.Factory.StartNew(() => GetTikers());
        }
    }
    public class GatePairs:IGetTraidPairs
    {
        Dictionary<string, string> IGetTraidPairs.ExeptionPairs { get; set; } = new Dictionary<string, string>();

        string site = "https://data.gateio.co/api2/1/marketlist";
        public BaseTraidPairModel GetPairs()
        {
            WebResponse resp = GetRequst.Requst(site);
            List<string> temp = new List<string>();
            using (StreamReader stream = new StreamReader
                     (resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var a = JsonConvert.DeserializeObject<GatePairsModel>(str);
                return a.ToBaseTraidPairs();
            }
        }

        public  Task<BaseTraidPairModel> GetTraidPairsAsync()
        {
            return  Task<BaseTraidPairModel>.Factory.StartNew(() => GetPairs());
        }
    }
    //string site = "http://data.gate.io/api2/1/marketinfo";
    public class GateInfo : IGetInfo
    {
        string GateInfoApi = "https://data.gateio.io/api2/1/coininfo";
        public BaseInfoModel GetInfo()
        {
            WebResponse resp = GetRequst.Requst(GateInfoApi);
            using (StreamReader stream = new StreamReader
                     (resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<GateInfoModel>(str);
                return res.ToBaseInfoModel();
            }
        }
        public Task<BaseInfoModel> GetInfoAsync()
        {
            return Task<BaseInfoModel>.Factory.StartNew(() => GetInfo());
        }
    }
    public class GateOrders : IGetOrders
    {
        public string orderBookApi = "http://data.gate.io/api2/1/orderBook/{0}";
        public string orderAllBookApi = "https://data.gateio.io/api2/1/orderBooks";
        public GateOrders()
        {

        }

        public BaseOrdersModel GetOrders(List<KeyValuePair<string, string>> arg)
        {
            Dictionary<string, BaseOrderModel> temp = new Dictionary<string, BaseOrderModel>();
            Dictionary<string, Task<BaseOrderModel>> tempAsync = new Dictionary<string, Task<BaseOrderModel>>();
            foreach (var i in arg)
            {
                tempAsync.Add(i.Key + AccseptCoins.SPLITER + i.Value, GetOrderAsync(i.Key, i.Value));
                temp.Add(i.Key + AccseptCoins.SPLITER + i.Value, new BaseOrderModel());
            }
            foreach (var i in tempAsync)
            {
                temp[i.Key] = i.Value.Result;
            }
            return new BaseOrdersModel() { Orders = temp };
        }
        public BaseOrderModel GetOrder(string MainCoinName, string SecondCoinName)
        {

            string site = String.Format(orderBookApi, MainCoinName.ToLower() + "_" + SecondCoinName.ToLower());

            WebResponse resp = GetRequst.Requst(site);

            using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();

                var res = JsonConvert.DeserializeObject<GateOrderModel>(str);
                return res.ToBaseOrderModel();
            }


        }
        public BaseOrdersModel GetAllOrders()
        {
            WebResponse resp = GetRequst.Requst(orderAllBookApi);
            using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string,GateOrderModel>>(str);
                return new BaseOrdersModel() { Orders = res.ToDictionary(x=>x.Key,y=>y.Value.ToBaseOrderModel())};
            }


        }
        public Task<BaseOrdersModel> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return Task<BaseOrdersModel>.Factory.StartNew(() => GetOrders(arg));
        }

        public Task<BaseOrderModel> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return Task<BaseOrderModel>.Factory.StartNew(() => GetOrder(MainCoinName, SecondCoinName));
        }

        public Task<BaseOrdersModel> GetAllOrdersAsync()
        {
            return Task<BaseOrdersModel>.Factory.StartNew(() => GetAllOrders());
        }
    }
    public class GateWallet : IWallet
    {
        public string Balance = "private/balances";
        public string Address = "private/depositAddress";
        public List<string> GetCoins()
        {
            List<string> tp = new List<string>();
            using (StreamReader stream = new StreamReader(
                   "prox/coins.txt"))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    tp.Add(line);
                }
            }
            return tp;
        }
        public GateWallet()
        {

        }

        public Dictionary<string, TransformBallans> GetBalances()
        {

            var str = GatePostRequst.PostString(Balance, "");

            Dictionary<string, TransformBallans> resault = new Dictionary<string, TransformBallans>();
            Dictionary<string, string> available = new Dictionary<string, string>();

            dynamic tp = JsonConvert.DeserializeObject(str);

            foreach (var item in tp.available)
            {
                available.Add(item.Name, item.Value);
            }
            foreach (var item in tp.locked)
            {
                if (available.ContainsKey(item.Name))
                {
                    resault.Add(item.Name, new TransformBallans(available[item.Name], item.Value));
                }
            }
            return resault;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var postData = new Dictionary<string, string>();
            foreach (var item in GetCoins())
            {
                postData.Add(item, "&currency=" + item);
            }
            var resault = new Dictionary<string, string>();
            foreach (var item in postData)
            {
                var str = GatePostRequst.PostString(Balance, item.Value);
                dynamic tp = JsonConvert.DeserializeObject(str);
                resault.Add(item.Key, tp.addr.Value);
            }
            return resault;
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
    public class GateTraid : ITrading
    {
        private string Sell = "private/sell";
        private string Buy = "private/buy";
        private string Withdrow = "private/withdraw";
        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>()
            {
                {"currencyPair",currencyPair + "_btc" },
                {"rate", pricePerCoin },
                {"amount", amountQuote }
            };
            string res = GatePostRequst.PostString(GetType(type), postData.ToHttpPostString());
            dynamic a = JsonConvert.DeserializeObject(res);
            return Convert.ToString(a.orderNumber);
        }
        public TransformWithdrow PostWihdrow(string currencyPair, string adrress, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>()
            {
                {"currencyPair",currencyPair},
                {"amount", amountQuote },
                {"adress", adrress }
            };
            string res = GatePostRequst.PostString(Withdrow, postData.ToHttpPostString());
            var temp = new TransformWithdrow() { Amount = amountQuote, Currency = currencyPair, Wallet = adrress, Date = DateTime.Now };
            return temp;
        }
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return Task<string>.Factory.StartNew(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote)
        {
            return Task<TransformWithdrow>.Factory.StartNew(() => PostWihdrow(currencyPair, adrress, amountQuote));
        }
        private string GetType(OrderType type)
        {
            if (type == OrderType.Buy)
            {
                return Buy;
            }
            else if (type == OrderType.Sell)
            {
                return Sell;
            }
            else
            {
                throw new Exception("Invalid Type");
            }
        }
    }

    public class Gate : Stock
    {
        public Gate()
        {
            HasCoinsName = true;
            HasWalletStatus = true;
            StockName = "Gate";
            Info = new GateInfo();
            Orders = new GateOrders();
            Ballans = new GateWallet();
            Traid = new GateTraid();
            TraidPairs = new GatePairs();
        }
    }

}
