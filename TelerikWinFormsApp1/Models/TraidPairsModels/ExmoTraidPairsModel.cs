using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Stocks;

namespace Cryptobot.Models.TraidPairsModels
{
    public class ExmoTraidPairField
    {
        public string min_quantity { get; set; }
        public string max_quantity { get; set; }
        public string min_price { get; set; }
        public string max_price { get; set; }
        public string max_amount { get; set; }
        public string min_amount { get; set; }
    }
    class ExmoTraidPairsModel: BaseTraidPairModel
    {
        public Dictionary<string, ExmoTraidPairField> Pairs { get; set; }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var tikers = new ExmoTiker().GetTikersAsync();
            var baseModel = new BaseTraidPairModel();
            foreach (var item in Pairs)
            {
                var Tiker = tikers.Result.Tikers.FirstOrDefault(y => y.Key == item.Key);
                if (Tiker.Key != null)
                {
                    var Ask = Tiker.Value.Ask;
                    var Bid = Tiker.Value.Bid;
                    baseModel.Pairs.Add(item.Key,
                        new BaseTraidPairField()
                        {
                            MarketCurrency = item.Key.Split('_')[0],
                            BaseCurrency = item.Key.Split('_')[1],
                            MinTradeSize = ToDecimal(item.Value.min_quantity),
                            MarketName = item.Key,
                            IsActive = true,
                            Ask = Ask,
                            Bid = Bid
                        });
                }
            }
            return baseModel;
        }
        private decimal ToDecimal(string str)
        {

            return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
}
