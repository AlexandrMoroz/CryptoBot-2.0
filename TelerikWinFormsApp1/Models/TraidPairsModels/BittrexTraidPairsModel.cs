using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Stocks;

namespace Cryptobot.Models.TraidPairsModels
{
    public class BittrexTraidPairsField
    {
        public string MarketCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public string MarketCurrencyLong { get; set; }
        public string BaseCurrencyLong { get; set; }
        public decimal MinTradeSize { get; set; }
        public string MarketName { get; set; }
        public bool IsActive { get; set; }
    }
    public class BittrexTraidPairsModel : BaseTraidPairModel
    {
        public string message;
        public bool success;
        public List<BittrexTraidPairsField> result;
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var tikers = new BittrexTicker().GetTikersAsync();
            var baseModel = new BaseTraidPairModel();
            foreach (var item in result)
            {
                var marketName = item.MarketCurrency + '-' + item.BaseCurrency;
                var marketNameBackword = item.BaseCurrency + '-' + item.MarketCurrency;
                var Ask = tikers.Result.Tikers.First(y => y.Key == marketNameBackword).Value.Ask;
                var Bid = tikers.Result.Tikers.First(y => y.Key == marketNameBackword).Value.Bid;
                baseModel.Pairs.Add(marketName,
                    new BaseTraidPairField()
                    {
                        MarketCurrency = item.MarketCurrency,
                        BaseCurrency = item.BaseCurrency,
                        MarketCurrencyLong = item.MarketCurrencyLong,
                        BaseCurrencyLong = item.BaseCurrencyLong,
                        MinTradeSize = item.MinTradeSize,
                        MarketName = item.MarketName,
                        IsActive = item.IsActive,
                        Ask = Ask,
                        Bid = Bid
                    });
            }

            return baseModel;
        }

    }
}
