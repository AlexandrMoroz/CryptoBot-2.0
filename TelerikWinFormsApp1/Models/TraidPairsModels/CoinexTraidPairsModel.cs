using System;
using System.Collections.Generic;
using System.Linq;
using Cryptobot.Models.InfoModels;
using Cryptobot.Stocks;

namespace Cryptobot.Models.TraidPairsModels
{

    public class CoinexTraidPairsField
    {
        public int MarketID { get; set; }
        public string MarketAssetName { get; set; }
        public string MarketAssetCode { get; set; }
        public int MarketAssetID { get; set; }
        public string MarketAssetType { get; set; }
        public string BaseCurrency { get; set; }
        public string BaseCurrencyCode { get; set; }
        public string BaseCurrencyID { get; set; }
        public bool Active { get; set; }
    }
    public class CoinexTraidPairsModel : BaseTraidPairModel
    {
        public int success { get; set; }
        public string message { get; set; }
        public List<CoinexTraidPairsField> result { get; set; }
        public override BaseTraidPairModel ToBaseTraidPairs() 
        {
            var tikers = new CoinexTicker().GetTikersAsync();
            var baseTraidPairs = new BaseTraidPairModel();
            foreach (var item in result)
            {
                var marketName = item.MarketAssetCode.ToUpper() + '-' + item.BaseCurrencyCode.ToUpper();
                var Tiker = tikers.Result.Tikers.FirstOrDefault(y => Convert.ToInt32(y.Key) == item.MarketID);
                if (Tiker.Key != null)
                {
                    var Ask = Tiker.Value.Ask;
                    var Bid = Tiker.Value.Bid;
                    baseTraidPairs.Pairs.Add(marketName,
                        new BaseTraidPairField()
                        {
                            Id = item.MarketID,
                            MarketCurrency = item.MarketAssetCode.ToUpper(),
                            BaseCurrency = item.BaseCurrencyCode.ToUpper(),
                            MarketCurrencyLong = item.MarketAssetName.ToUpper(),
                            BaseCurrencyLong = item.BaseCurrency.ToUpper(),
                            MarketName = marketName,
                            IsActive = item.Active,
                            Ask = Ask,
                            Bid = Bid
                        });
                }
            }

            return baseTraidPairs;
        }
    }
}