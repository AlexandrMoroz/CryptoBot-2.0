using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.TraidPairsModels
{
    public class KucoinTraidPairsField
    {
        public string coinType { get; set; }
        public string symbol { get; set; }
        public bool trading { get; set; }
        public string coinTypePair { get; set; }
        public decimal feeRate { get; set; }

    }
    public class KucoinTraidPairsModel:BaseTraidPairModel
    {
        public bool success { get; set; }
        public List<KucoinTraidPairsField> data { get; set; }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var baseTraidPairs = new BaseTraidPairModel();
            baseTraidPairs.Pairs = data.ToDictionary(x => x.symbol, 
            y => new BaseTraidPairField() {
                MarketCurrency = y.coinType,
                BaseCurrency = y.coinTypePair,
                MarketName = y.symbol,
                IsActive = y.trading,
            });
            return baseTraidPairs;
        }
    }
}
