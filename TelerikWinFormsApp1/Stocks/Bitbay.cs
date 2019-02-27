using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Interfesse;
using Cryptobot.Models.InfoModels;
using Cryptobot.Models.OrdersModels;
using Cryptobot.Models.TraidPairsModels;

namespace Cryptobot.Stocks
{
    public class BitbayTraidPairs : IGetTraidPairs
    {
        Dictionary<string, string> IGetTraidPairs.ExeptionPairs { get; set; } = new Dictionary<string, string>();

        string site = "https://api.bitbay.net/rest/trading/ticker";
        public BaseTraidPairModel GetTradePairs()
        {

            WebResponse resp = GetRequst.Requst(site);

            using (StreamReader stream = new StreamReader
                      (resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var a = JsonConvert.DeserializeObject<BitbayTraidPairsModel>(str);
                return a.ToBaseTraidPairs();
            }
        }
        public Task<BaseTraidPairModel> GetTraidPairsAsync()
        {
            return Task<BaseTraidPairModel>.Factory.StartNew(() => GetTradePairs());
        }


    }
    public class BitbayInfo : IGetInfo
    {
        private string info = "info";
        public BaseInfoModel GetInfo()
        {

            var str = BitbayPostRequst.PostString(info);
            var a = JsonConvert.DeserializeObject<BitbayInfoModel>(str.Result);
            return a.ToBaseInfoModel();

        }
        public Task<BaseInfoModel> GetInfoAsync()
        {
            return Task<BaseInfoModel>.Factory.StartNew(() => GetInfo());
        }
    }


    public class BitbayOrder : IGetOrders
    {
        string site = "https://bitbay.net/API/Public/{0}{1}/orderbook.json";
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
            return new BittrexOrdersModel(temp).ToBaseOrderModel();
        }

        public BaseOrderModel GetOrder(string MainCoinName, string SecondCoinName)
        {
            string temp = String.Format(site, MainCoinName, SecondCoinName);

            WebResponse resp = GetRequst.Requst(temp);
            using (StreamReader stream = new StreamReader(
                    resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<BitbayOrderModel>(str);
                return res.ToBaseOrderModel();
            }
        }

        public BaseOrdersModel GetAllOrders()
        {
            var pairs = new List<KeyValuePair<string, string>>();
            var info = new BitbayTraidPairs().GetTraidPairsAsync();
            foreach (var item in info.Result.Pairs)
            {
                if (item.Value.IsActive == false)
                {
                    continue;
                }
                pairs.Add(new KeyValuePair<string, string>(item.Value.MarketCurrency, item.Value.BaseCurrency));
            }
            var orders = GetOrders(pairs);
            return orders;
        }

        public Task<BaseOrdersModel> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return Task<BaseOrdersModel>.Factory.StartNew(() => GetOrders(arg));
        }

        public Task<BaseOrderModel> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return Task<BaseOrderModel>.Factory.StartNew(() => GetOrder(MainCoinName, SecondCoinName));
        }
        /// <summary>
        /// Make single requst for every market.
        /// It take about 1:10 min.
        /// </summary>
        /// <returns></returns>
        public Task<BaseOrdersModel> GetAllOrdersAsync()
        {
            return Task<BaseOrdersModel>.Factory.StartNew(() => GetAllOrders());
        }
    }
    //Not Implemented
    public class BitbayWallet : IWallet
    {

        const string ApiCallGetBalances = "account/getbalances";
        const string ApiCallGetAdrress = "account/getdepositaddress";
        public Dictionary<string, TransformBallans> GetBalances()
        {
            Dictionary<string, TransformBallans> temp = new Dictionary<string, TransformBallans>();
            var resp = BittrexPostRequst.PostString(ApiCallGetBalances, "");
            dynamic jObject = JObject.Parse(resp);

            foreach (var item in jObject.result)
            {
                temp.Add(item.Currency.Value, new TransformBallans(Convert.ToDecimal(item.Available.Value), Convert.ToDecimal(item.Pending.Value)));
            }
            return temp;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var temp = new Dictionary<string, string>();
            var lt = AccseptCoins.GetCoins();
            var postData = new List<string>();
            foreach (var item in lt)
            {
                postData.Add("&currency=" + item);
            }
            foreach (var item in postData)
            {
                var resp = BittrexPostRequst.PostString(ApiCallGetAdrress, item);
                dynamic jObject = JObject.Parse(resp);
                temp.Add(jObject.Currency, jObject.Address);
            }
            return temp;
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
    public class BitbayTraid : ITrading
    {
        private string Buy = "market/buylimit";
        private string Sell = "market/selllimit";
        private string Withdraw = "account/withdraw";
        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>()
            {
                {"market", "BTC-"+currencyPair},
                {"quantity",amountQuote },
                {"rate",pricePerCoin }
            };
            var resp = BittrexPostRequst.PostString(GetOrderType(type), postData.ToHttpPostString());
            dynamic jObject = JObject.Parse(resp);
            return Convert.ToString(jObject.uuid);
        }
        public TransformWithdrow PostWihdrow(string currencyPair, string adrress, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>()
            {
                {"currency", currencyPair},
                {"quantity",amountQuote },
                {"address",adrress }
            };
            var resp = BittrexPostRequst.PostString(Withdraw, postData.ToHttpPostString());
            dynamic jObject = JObject.Parse(resp);
            return Convert.ToString(jObject.uuid);
        }
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return Task<string>.Factory.StartNew(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote)
        {
            return Task<TransformWithdrow>.Factory.StartNew(() => PostWihdrow(currencyPair, adrress, amountQuote));
        }
        private string GetOrderType(OrderType arg)
        {
            switch (arg)
            {
                case OrderType.Buy:
                    return Buy;

                case OrderType.Sell:
                    return Sell;

            }
            throw new Exception("Wrong OrderType");
        }

    }
    public class Bitbay : Stock
    {
        public Bitbay()
        {
            HasCoinsName = true;
            HasWalletStatus = false;
            base.StockName = "Bitbay";
            base.Info = new BitbayInfo();
            base.Orders = new BitbayOrder();
            base.Ballans = new BitbayWallet();
            base.Traid = new BitbayTraid();
            base.TraidPairs = new BitbayTraidPairs();
        }
    }
}
