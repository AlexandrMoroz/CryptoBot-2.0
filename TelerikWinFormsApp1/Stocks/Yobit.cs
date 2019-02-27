using HtmlAgilityPack;
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
    public class YobitTraidPairs: IGetTraidPairs
    {
        private string site = "https://yobit.net/api/3/depth/{0}error_pair?ignore_invalid=1";
        private  string info = "https://yobit.net/api/3/info";
        Dictionary<string, string> IGetTraidPairs.ExeptionPairs { get; set; } = new Dictionary<string, string>();
        public YobitTraidPairs()
        {

        }



        public BaseTraidPairModel GetTraidPairs()
        {
            BaseTraidPairModel pairsModel ;
            WebResponse resp = GetRequst.Requst(info);
            using (StreamReader stream = new StreamReader(
                      resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                pairsModel = JsonConvert.DeserializeObject<YobitTraidPairsModel>(str).ToBaseTraidPairs();
            }
            string format = "";
            for (int i = 1; i < pairsModel.Pairs.Count; i++)
            {
                    format += pairsModel.Pairs.ElementAt(i).Key + "-";
            }
            WebResponse resp2 = GetRequst.Requst(String.Format(site, format));
            using (StreamReader stream = new StreamReader(
                      resp2.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, YobiTickerField>>(str);
                var model = new YobitTraidPairsMainModel(res, pairsModel.ToBaseTraidPairs());
                 return model.ToBaseTraidPairs();
            }
        }

        Task<BaseTraidPairModel> IGetTraidPairs.GetTraidPairsAsync()
        {
            return Task<BaseTraidPairModel>.Factory.StartNew(() => GetTraidPairs());
        }
    }
    public class YobitInfo : IGetInfo
    {
        string site = "https://yobit.net/api/3/info";

        public YobitInfo()
        {
        }

        public BaseInfoModel GetInfo()
        {
            BaseInfoModel baseInfoModel = new BaseInfoModel();    
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                      resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<YobitInfoModel>(str);

                return res.ToBaseInfoModel();
            }
        }

        public Task<BaseInfoModel> GetInfoAsync()
        {
            return Task<BaseInfoModel>.Factory.StartNew(() => GetInfo());
        }
    }
    public class YobitOrders : IGetOrders
    {
        string end = @"?limit=30";
        string start = "https://yobit.net/api/3/depth/";
        public class Field
        {

        }
        public YobitOrders()
        {

        }
        public BaseOrdersModel GetOrders(List<KeyValuePair<string, string>> arg)
        {
            string allPairs = "";
            for (int i = 0; i <= arg.Count; i++)
            {
                if (i == arg.Count)
                {
                    allPairs += arg.ElementAt(i).Key;

                }
                else
                {
                    allPairs += arg.ElementAt(i).Key + '-';
                }

            }
            string site = start + allPairs + end;
            BaseOrderModel baseOrder = new BaseOrderModel();
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, YobitOrderField>>(str);
                var model = new YobitAllOrderModel();
                model.order = res;
                return model.ToBaseOrderModel();
            }
        }
        public BaseOrderModel GetOrder(string MainCoinName, string SecondCoinName)
        {
            string site = start + MainCoinName.ToLower() + "_" + SecondCoinName.ToLower() + end;
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();

                var res = JsonConvert.DeserializeObject<Dictionary<string, YobitOrderField>>(str);
                var model = new YobitOrderModel();
                model.order = res;
                return model.ToBaseOrderModel();
            }
        }
      
        public BaseOrdersModel GetAllOrder()
        {
            var pairs = new YobitInfo().GetInfoAsync().Result;
            string allPairs="";
            for (int i = 0; i <= pairs.CoinsInfo.Count; i++)
            {
                if (i== pairs.CoinsInfo.Count)
                {
                    allPairs += pairs.CoinsInfo.ElementAt(i).Key;

                }
                else
                {
                    allPairs += pairs.CoinsInfo.ElementAt(i).Key+'-';
                }
                
            } 
            string site = start + allPairs + end;
            BaseOrderModel baseOrder = new BaseOrderModel();
            WebResponse resp = GetRequst.Requst(site);
            using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, YobitOrderField>>(str);
                var model = new YobitAllOrderModel();
                model.order = res;
                return model.ToBaseOrderModel();
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
            return Task<BaseOrdersModel>.Factory.StartNew(() => GetAllOrder());
        }




    }
    public class YoubitWallet : IWallet
    {
        public string Address = "GetDepositAddress";
        public string Balance = "getInfo";
        public Dictionary<string, TransformBallans> GetBalances()
        {
            var list = Helpers.GetCoins();
            var result = new Dictionary<string, TransformBallans>();
            var temp = YoubitPostRequst.PostString(Balance, "");
            if (!temp.Contains("funds"))
            {
                foreach (var item in list)
                {
                    result.Add(item, new TransformBallans(0, 0));
                }
            }
            else
            {
                var res = JObject.Parse(temp);
                foreach (var item in list)
                {
                    var avaible = Convert.ToDecimal(res["return"]["funds"][item]);
                    var onorders = Convert.ToDecimal(res["return"]["funds_incl_orders"][item]) - avaible;
                    result.Add(item, new TransformBallans(avaible, onorders));
                }

            }

            return result;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var list = Helpers.GetCoins();
            var result = new Dictionary<string, string>();
            foreach (var item in list)
            {
                var postData = new Dictionary<string, object>();
                postData.Add("coinName", item);
                postData.Add("need_new", 0);
                var temp = YoubitPostRequst.PostString(Address, postData.ToHttpPostString());
                var res = JObject.Parse(temp);
                result.Add(item, res["return"]["address"].ToString());
            }
            return result;
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
    public class YobitTraid : ITrading
    {
        public string Trade = "Trade";
        public string Withdrow = "WithdrawCoinsToAddress";
        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>();
            postData.Add("pair", currencyPair.ToLower() + "_btc");
            postData.Add("type", GetOrderType(type));
            postData.Add("rate", pricePerCoin);
            postData.Add("amount", amountQuote);
            var temp = YoubitPostRequst.PostString(Trade, postData.ToHttpPostString());
            var res = JObject.Parse(temp);
            return res["return"]["order_id"].ToString();
        }
        public TransformWithdrow PostWihdrow(string currencyPair, string adrress, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>();
            postData.Add("coinName", currencyPair);
            postData.Add("amount", amountQuote);
            postData.Add("address", adrress);
            var temp = YoubitPostRequst.PostString(Withdrow, postData.ToHttpPostString());
            var res = JObject.Parse(temp);
            return new TransformWithdrow(0, currencyPair, adrress, amountQuote, DateTime.Now);
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
                    return "buy";

                case OrderType.Sell:
                    return "sell";

            }
            throw new Exception("Wrong OrderType");
        }
    }

    public class Yobit : Stock
    {
        public Yobit()
        {
            StockName = "Yobit";
            Info = new YobitInfo();
            Orders = new YobitOrders();
            Ballans = new YoubitWallet();
            Traid = new YobitTraid();
            TraidPairs = new YobitTraidPairs();
        }
    }
}
