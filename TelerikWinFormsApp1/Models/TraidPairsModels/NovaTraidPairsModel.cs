using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Stocks;

namespace Cryptobot.Models.TraidPairsModels
{
    public class NovaTraidPairsField
    {
        public int marketid { get; set; }
        public string currency { get; set; }
        public string marketname { get; set; }
        public string basecurrency { get; set; }
        public string coinTypePair { get; set; }
        public decimal ask { get; set; }
        public decimal bid { get; set; }
        public bool disabled { get; set; }

    }
    public class NovaTraidPairsModel:BaseTraidPairModel
    {
        public string success { get; set; }
        public string message { get; set; }
        public List<NovaTraidPairsField> markets { get; set; }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var info = new NovaInfo().GetInfoAsync();
            var baseTraidPairs = new BaseTraidPairModel();
            baseTraidPairs.Pairs = markets.ToDictionary(x => x.currency+'-'+ x.basecurrency,
            y => new BaseTraidPairField()
            {
                Id = y.marketid,
                MarketCurrency = y.currency,
                BaseCurrency = y.basecurrency,
                MarketCurrencyLong= info.Result.CoinsInfo.First(x => x.Key == y.basecurrency).Value.Name.ToUpper(),
                BaseCurrencyLong = info.Result.CoinsInfo.First(x=>x.Key == y.currency).Value.Name.ToUpper(),
                MarketName = y.currency + '-' + y.basecurrency,
                IsActive = y.disabled,
                Ask=y.ask,
                Bid=y.bid
                
            });
            return baseTraidPairs;
        }
    }
}
