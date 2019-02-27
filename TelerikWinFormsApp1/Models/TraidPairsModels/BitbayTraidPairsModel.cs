using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.TraidPairsModels
{

    public class BitbayTraidPairsModel : BaseTraidPairModel
    {
        public string status { get; set; }
        public Dictionary<string, dynamic> items { get; set; }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var basePairs = new BaseTraidPairModel();
            var symbolName = GetSymbolAndName();
            foreach (var item in items)
            {
                var MarketCoin = item.Key.Split('-')[0];
                var BaseCoin = item.Key.Split('-')[1];
                var marketLong = symbolName.First(x => x.Key == MarketCoin).Value;
                var baseLong = symbolName.First(x => x.Key == BaseCoin).Value;
                var Ask = item.Value.lowestAsk.Value;
                var Bid = item.Value.highestBid.Value;
                basePairs.Pairs.Add(item.Key, new BaseTraidPairField()
                {
                    MarketCurrency = MarketCoin,
                    BaseCurrency = BaseCoin,
                    MarketCurrencyLong = marketLong,
                    BaseCurrencyLong = baseLong,
                    Ask = ToDecimal(Ask),
                    Bid = ToDecimal(Bid),
                    IsActive = true
                });
            }
            return basePairs;

        }
        private Dictionary<string, string> GetSymbolAndName()
        {

            var data = new Dictionary<string, string>();
            var url = "https://bitbay.net/en";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var table = doc.DocumentNode.SelectNodes("//div[@class='flex-currency']/div[@class='currency-item']/a/p");
            for (int i = 0; i < table.Count; i++)
            {
                var symbol = table[i].ChildNodes[0].InnerText;
                var name = table[i].ChildNodes[1].InnerText;
                data.Add(symbol, name);
            }

            return data;
        }
        private decimal ToDecimal(string str)
        {
            if (str != null)
            {
                return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            else
            {
                return 0;
            }
            
        }
    }
}
