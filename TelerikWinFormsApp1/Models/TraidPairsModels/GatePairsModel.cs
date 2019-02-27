using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Stocks;

namespace Cryptobot.Models.TraidPairsModels
{
    public class GatePairsField
    {
        public int no { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string pair { get; set; }
        public string curr_a { get; set; }
        public string curr_b { get; set; }
    }
    public class GatePairsModel : BaseTraidPairModel
    {
        private Dictionary<string, string> StockMinCoins { get; set; }

        public GatePairsModel()
        {
            StockMinCoins = new Dictionary<string, string>();
            StockMinCoins.Add("USDT", "Tether USDT");
            StockMinCoins.Add("BTC", "Bitcoin");
            StockMinCoins.Add("CNYX ", "CNYX");
            StockMinCoins.Add("ETH", "Ethereum ");
            StockMinCoins.Add("QTUM", "Qtum");
            StockMinCoins.Add("SNET", "Snetwork");
        }
        public bool result { get; set; }
        public List<GatePairsField> data { get; set; }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var basePairsModel = new BaseTraidPairModel();
            var Tikers = new GateTiker().GetTikersAsync();
            foreach (var item in data)
            {
                var marketName = item.pair;
                var Tiker = Tikers.Result.Tikers.FirstOrDefault(y => y.Key == marketName);
                if (Tiker.Key != null)
                {
                          basePairsModel.Pairs.Add(item.curr_a+'-'+ item.curr_b,
                            new BaseTraidPairField()
                            {
                                MarketCurrency = item.curr_a,
                                BaseCurrency = item.curr_b,
                                MarketCurrencyLong = item.name.ToUpper(),
                                BaseCurrencyLong = StockMinCoins[item.curr_b.ToUpper()].ToUpper(),
                                Ask = Tiker.Value.Ask,
                                Bid = Tiker.Value.Bid
                            });
                }
            }
            return basePairsModel;

          
        }
    }
}
