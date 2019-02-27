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
using Cryptobot.Models.OrdersModels;
using Cryptobot.Models.InfoModels;
using Cryptobot.Models.TraidPairsModels;

namespace Cryptobot.Stocks
{
    public class PoloniexTraidPairs : IGetTraidPairs
    {
        Dictionary<string, string> IGetTraidPairs.ExeptionPairs { get; set; } = new Dictionary<string, string>();


        public BaseTraidPairModel GetTraidPairs()
        {
            string site = "https://poloniex.com/public?command=returnTicker";
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                   resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, PoloniexTraidPairsField>>(str);
                var model = new PoloniexTraidPairsModel();
                model.pairs = res;
                return model.ToBaseTraidPairs();
            }
        }
        public Task<BaseTraidPairModel> GetTraidPairsAsync()
        {
            return  Task<BaseTraidPairModel>.Factory.StartNew(() => GetTraidPairs());
        }
    }

    public class PoloniexInfo : IGetInfo
    {
        public PoloniexInfo()
        {

        }
        public BaseInfoModel GetInfo()
        {
            string site = "https://poloniex.com/public?command=returnCurrencies";
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                   resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string,PoloniexInfoField>>(str);
                var model = new PoloniexInfoModel();
                model.pairs = res;
                return model.ToBaseInfoModel();
            }

        }

        public Task<BaseInfoModel> GetInfoAsync()
        {
            return  Task<BaseInfoModel>.Factory.StartNew(() => GetInfo());
        }

    }
    public class PoloniexOrders : IGetOrders
    {
        public PoloniexOrders()
        {

        }
        public BaseOrdersModel GetAllOrders()
        {
            string site = "https://poloniex.com/public?command=returnOrderBook&currencyPair=all";
            WebResponse resp = GetRequst.Requst(site);

            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, PoloniexOrderModel>>(str);
                var model = new PoloniexAllOrderModel(res);
                                    return model.ToBaseOrderModel();
            }
        }
        public BaseOrdersModel GetOrders(List<KeyValuePair<string, string>> arg)
        {

            Dictionary<string, BaseOrderModel> temp = new Dictionary<string, BaseOrderModel>();
            Dictionary<string, Task<BaseOrderModel>> tempAsync = new Dictionary<string, Task<BaseOrderModel>>();
            foreach (var i in arg)
            {
                tempAsync.Add(i.Key + AccseptCoins.SPLITER + i.Value, GetOrderAsync(i.Key, i.Value));
            }
            foreach (var i in tempAsync)
            {
                temp[i.Key] = i.Value.Result;
            }
            return new BaseOrdersModel() { Orders = temp };
        }
        public BaseOrderModel GetOrder(string MainCoinName, string SecondCoinName)
        {
            string site = String.Format("https://poloniex.com/public?command=returnOrderBook&currencyPair={0}", SecondCoinName + "_" + MainCoinName);

            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
             resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();

                var res = JsonConvert.DeserializeObject<PoloniexOrderModel>(str);
                return  res.ToBaseOrderModel();
            }
   
        }
        public  Task<BaseOrdersModel> GetAllOrdersAsync()
        {
            return  Task<BaseOrdersModel>.Factory.StartNew(() => GetAllOrders());
        }
        public  Task<BaseOrdersModel> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return  Task<BaseOrdersModel>.Factory.StartNew(() => GetOrders(arg));
        }
        public  Task<BaseOrderModel> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return  Task<BaseOrderModel>.Factory.StartNew(() => GetOrder(MainCoinName, SecondCoinName));
        }
    }

    public class PoloniexWalletField
    {
        public decimal available;
        public decimal onOrders;
        public decimal btcValue;
    }
    public class PoloniexWallet : IWallet
    {
        public Dictionary<string, TransformBallans> GetBalances()
        {
            var postData = new Dictionary<string, object>();
            postData.Add("command", "returnCompleteBalances");
            postData.Add("nonce", PoloniexPostRequst.GetCurrentHttpPostNonce());

            var str = PoloniexPostRequst.PostString("tradingApi", postData.ToHttpPostString());
            Dictionary<string, TransformBallans> temp = new Dictionary<string, TransformBallans>();
            var t = JsonConvert.DeserializeObject<Dictionary<string, PoloniexWalletField>>(str);
            temp = t.ToDictionary(x => x.Key, y => new TransformBallans(y.Value));
            return temp;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var postData = new Dictionary<string, object>();
            postData.Add("command", "returnDepositAddresses");
            postData.Add("nonce", PoloniexPostRequst.GetCurrentHttpPostNonce());

            var str = PoloniexPostRequst.PostString("tradingApi", postData.ToHttpPostString());
            Dictionary<string, string> temp = new Dictionary<string, string>();
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
    public class PoloniexTraid : ITrading
    {

        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var postData = new Dictionary<string, object> {
                { "command", GetOrderType(type)},
                { "currencyPair", currencyPair },
                { "rate", Convert.ToString(pricePerCoin) },
                { "amount", Convert.ToString(amountQuote) },
                { "nonce", PoloniexPostRequst.GetCurrentHttpPostNonce()}
            };

            var str = PoloniexPostRequst.PostString("tradingApi", postData.ToHttpPostString());
            var tp = JsonConvert.DeserializeObject<JObject>(str);
            return tp.Value<string>("orderNumber");
        }
        public TransformWithdrow PostWithdrow(string currencyPair, string adrress, decimal amountQuote)
        {
            var postData = new Dictionary<string, object> {
                { "command", "withdraw"},
                { "currencyPair", currencyPair },
                { "adrress", adrress },
                { "amount", Convert.ToString(amountQuote) },
                { "nonce", PoloniexPostRequst.GetCurrentHttpPostNonce()}
            };

            var str = PoloniexPostRequst.PostString("tradingApi", postData.ToHttpPostString());
            var tp = JsonConvert.DeserializeObject<JObject>(str);
            return new TransformWithdrow(tp.Value<int>("id"), tp.Value<string>("currency"), tp.Value<string>("address"), tp.Value<decimal>("amount"), tp.Value<DateTime>("date"));
        }

        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return Task<string>.Factory.StartNew(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote)
        {
            return Task<TransformWithdrow>.Factory.StartNew(() => PostWithdrow(currencyPair, adrress, amountQuote));
        }

        private string GetOrderType(OrderType arg)
        {
            switch (arg)
            {
                case OrderType.Buy:
                    return "buy";

                case OrderType.Sell:
                    return "sell";

            }
            throw new Exception("Wrong OrderType");
        }


    }
    public class Poloniex : Stock
    {
        public Poloniex()
        {
            HasCoinsName = true;
            HasWalletStatus = true;
            StockName = "Poloniex";
            Info = new PoloniexInfo();
            Orders = new PoloniexOrders();
            Ballans = new PoloniexWallet();
            Traid = new PoloniexTraid();
            TraidPairs = new PoloniexTraidPairs();
        }
    }
}
