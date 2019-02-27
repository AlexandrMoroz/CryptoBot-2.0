using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cryptobot.Interfesse;
using Cryptobot.Models.InfoModels;
using Cryptobot.Models.OrdersModels;
using Cryptobot.Models.TickerModels;
using Cryptobot.Models.TraidPairsModels;
using Cryptobot.Models.WalletModels;

namespace Cryptobot.Stocks
{

    public class LiveCoinPairs : IGetTraidPairs
    {
        Dictionary<string, string> IGetTraidPairs.ExeptionPairs { get; set; } = new Dictionary<string, string>();

        private string site = "https://api.livecoin.net/exchange/ticker";

        public BaseTraidPairModel GetTraidPairs()
        {
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                   resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<List<LiveCoinTraidPairField>>(str);
                var model = new LiveCoinTraidPairModel();
                model.tikers = res;
                return model.ToBaseTraidPairs();
            }
        }
        public  Task<BaseTraidPairModel> GetTraidPairsAsync()
        {
            return  Task<BaseTraidPairModel>.Factory.StartNew(() => GetTraidPairs());
        }
    }

    public class LiveCoinInfo : IGetInfo
    {
        public LiveCoinInfo()
        {

        }
        public  BaseInfoModel GetInfo()
        {
            string site = "https://api.livecoin.net/info/coinInfo";

            WebResponse resp = GetRequst.Requst(site);

            using (StreamReader stream = new StreamReader(
                    resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<LiveCoinInfoModel>(str);
                if (res.success == false)
                {
                    throw new Exception("LiveCoin info request not Work");
                }       
                return res.ToBaseInfoModel();
            }

        }
        public Task<BaseInfoModel> GetInfoAsync()
        {
            return Task<BaseInfoModel>.Factory.StartNew(() => GetInfo());
        }
    }
    public class LiveCoinOrders : IGetOrders
    {
        private string AllOrderBook = "exchange/all/order_book";
        private string OrderByCoin = "exchange/order_book?currencyPair={0}";
        private string Api = "https://api.livecoin.net/";
        public LiveCoinOrders()
        {

        }

        public BaseOrderModel GetOrder(string MainCoinName, string SecondCoinName)
        {

            string site = String.Format(Api + OrderByCoin, MainCoinName + "/" +  SecondCoinName);

            WebResponse resp = LiveCoinGetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<LiveCoinOrderModel>(str);
                return res.ToBaseOrderModel();
            }
        }
        public BaseOrdersModel GetAllOrders()
        {

            string site = String.Format(Api + AllOrderBook);

            WebResponse resp = LiveCoinGetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, LiveCoinOrderModel>>(str);
                var model = new LiveCoinAllOrderModel();
                model.AllOrders = res;
                return model.ToBaseOrderModel();
            }
        }
        public  BaseOrdersModel GetOrders(List<KeyValuePair< string, string>> arg)
        {
            Dictionary<string, BaseOrderModel> temp = new Dictionary<string, BaseOrderModel>();
            Dictionary<string, Task<BaseOrderModel>> tempAsync = new Dictionary<string, Task<BaseOrderModel>>();
            foreach (var i in arg)
            {
                tempAsync.Add(i.Key + AccseptCoins.SPLITER + i.Value,  GetOrderAsync(i.Key, i.Value));
            }
            foreach (var i in tempAsync)
            {
                temp[i.Key] = i.Value.Result;
            }
            return new BaseOrdersModel() { Orders = temp };
        }

        public  Task<BaseOrdersModel> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return  Task<BaseOrdersModel>.Factory.StartNew(() => GetOrders(arg));
        }

        public  Task<BaseOrderModel> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return  Task<BaseOrderModel>.Factory.StartNew(() => GetOrder(MainCoinName, SecondCoinName));
        }

        public  Task<BaseOrdersModel> GetAllOrdersAsync()
        {
            return  Task<BaseOrdersModel>.Factory.StartNew(()=>GetAllOrders());
        }

    }

    public class LiveCoinWallet : IWallet
    {

        private string Balances = "payment/balances";
        private string Addresses = "payment/get/address";
        public Dictionary<string, TransformBallans> GetBalances()
        {
            WebResponse response = LiveCoinGetRequst.AuthRequst(Balances, "");
            Dictionary<string, TransformBallans> temp = new Dictionary<string, TransformBallans>();
            using (StreamReader stream = new StreamReader(
                   response.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var resalt = JsonConvert.DeserializeObject<LiveCoinBalanceModel>(str);
               
                return temp;
            }
        }

        public Dictionary<string, string> GetDepositAddresses()
        {
            WebResponse response = LiveCoinGetRequst.AuthRequst(Addresses, "");
            Dictionary<string, string> temp;
            using (StreamReader stream = new StreamReader(
                   response.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

            }
            return temp;
        }

        public Task<Dictionary<string, TransformBallans>> GetBalancesAsync()
        {
            return new Task<Dictionary<string, TransformBallans>>(() => GetBalances());
        }

        public Task<Dictionary<string, string>> GetDepositAddressesAsync()
        {
            return new Task<Dictionary<string, string>>(() => GetDepositAddresses());
        }


    }
    public class LiveCoinTraide : ITrading
    {
        public class Field
        {
            public int id;
            public decimal amount;
            public string currency;
            public string wallet;
            public string date;
        }
        private static string Exchange = "exchange/";
        private string GetOrderType(OrderType arg)
        {
            switch (arg)
            {
                case OrderType.Buy:
                    return "buylimit";

                case OrderType.Sell:
                    return "selllimit";

            }
            throw new Exception("Wrong OrderType");
        }
        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            Dictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("currencyPair", currencyPair);
            postdata.Add("price", pricePerCoin);
            postdata.Add("quantity", amountQuote);

            var res = LiveCoinPostRequst.PostString(Exchange + GetOrderType(type), postdata.ToHttpPostString());
            dynamic jObject = JObject.Parse(res);
            return Convert.ToString(jObject.id);
        }
        public TransformWithdrow PostWihdrow(string currencyPair, string address, decimal amount)
        {
            Dictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("currency", currencyPair);
            postdata.Add("wallet", address);
            postdata.Add("amount", amount);
            var str = LiveCoinPostRequst.PostString("payment/out/coin", postdata.ToHttpPostString());
            Field temp = JsonConvert.DeserializeObject<Field>(str);
            TransformWithdrow tr = new TransformWithdrow(temp.id, temp.currency, temp.wallet, temp.amount, Convert.ToDateTime(temp.date));
            return tr;
        }
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return new Task<string>(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string address, decimal amount)
        {
            return new Task<TransformWithdrow>(() => PostWihdrow(currencyPair, address, amount));
        }
    }
    public class LiveCoin : Stock
    {
        public LiveCoin()
        {
            HasCoinsName = true;
            HasWalletStatus = true;
            StockName = "LiveCoin";
            Orders = new LiveCoinOrders();
            Info = new LiveCoinInfo();
            Ballans = new LiveCoinWallet();
            Traid = new LiveCoinTraide();
            TraidPairs = new LiveCoinPairs();
        }
    }
}
