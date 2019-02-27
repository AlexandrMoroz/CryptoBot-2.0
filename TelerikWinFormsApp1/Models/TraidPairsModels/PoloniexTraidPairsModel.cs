using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Models.InfoModels;
using Cryptobot.Stocks;
namespace Cryptobot.Models.TraidPairsModels
{
    public class PoloniexTraidPairsField
    {
        public int isFrozen { get; set; }
        public string lowestAsk { get; set; }
        public string highestBid { get; set; }
    }
    public class PoloniexTraidPairsModel:BaseTraidPairModel
    {
        public Dictionary<string,PoloniexTraidPairsField> pairs { get; set; }
        private BaseInfoModel info { get; set; }
        public PoloniexTraidPairsModel()
        {
            info = new PoloniexInfo().GetInfoAsync().Result;
        }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var baseTraidPairs = new BaseTraidPairModel();
            foreach (var item in pairs)
            {
                if (!baseTraidPairs.Pairs.ContainsKey(item.Key))
                {
                    var BaseCurrency = item.Key.Split('_')[0].ToUpper();
                    var MarketCurrency = item.Key.Split('_')[1].ToUpper();
                    var MarketName = MarketCurrency + '-'+ BaseCurrency;
                    baseTraidPairs.Pairs.Add(MarketName, new BaseTraidPairField() {
                        MarketCurrency = MarketCurrency,
                        BaseCurrency = BaseCurrency,
                        MarketCurrencyLong = GetName(MarketCurrency),
                        BaseCurrencyLong = GetName(BaseCurrency),
                        IsActive = item.Value.isFrozen == 0 ? false : true,
                        Ask= ToDecimal(item.Value.lowestAsk),
                        Bid= ToDecimal(item.Value.highestBid)
                        
                    });
                }

            }
            return baseTraidPairs;
        }
        private string GetName( string arg)
        {
            return info.CoinsInfo.First(x => x.Key == arg).Value.Name.ToUpper();
        }
        private decimal ToDecimal(string str)
        {

            return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
}
