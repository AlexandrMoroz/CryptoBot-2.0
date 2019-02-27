using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Stocks;

namespace Cryptobot.Models.TraidPairsModels
{
    public class LiveCoinTraidPairField
    {
        public string cur { get; set; }
        public string symbol { get; set; }
        public decimal best_bid { get; set; }
        public decimal best_ask { get; set; }
    }
    public class LiveCoinTraidPairModel: BaseTraidPairModel
    {
        public List<LiveCoinTraidPairField> tikers { get; set; }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var info = new LiveCoinInfo().GetInfoAsync();
            var baseTraidPairs = new BaseTraidPairModel();
            foreach (var item in tikers)
            {
                var marketCurrency = item.symbol.Split('/')[0];
                var baseCurrency = item.symbol.Split('/')[1];
                var MarketCurrencyLong = info.Result.CoinsInfo.FirstOrDefault(x => x.Key == marketCurrency);

                var BaseCurrencyLong = info.Result.CoinsInfo.FirstOrDefault(x => x.Key == baseCurrency);
                var status= info.Result.CoinsInfo.FirstOrDefault(x => x.Key == baseCurrency);
            
                
                baseTraidPairs.Pairs.Add( marketCurrency+'-'+ baseCurrency,
                        new BaseTraidPairField()
                        {
                            MarketCurrency = marketCurrency,
                            BaseCurrency = baseCurrency,
                            MarketCurrencyLong = MarketCurrencyLong.Key == null ? marketCurrency : MarketCurrencyLong.Value.Name.ToUpper(),
                            BaseCurrencyLong = BaseCurrencyLong.Key == null? baseCurrency:BaseCurrencyLong.Value.Name.ToUpper(),
                            MarketName = marketCurrency + '-' + baseCurrency,
                            IsActive = status.Key == null ? true: status.Value.WalletStatus,
                            Ask = item.best_ask,
                            Bid = item.best_bid
                        });
                }
           

            return baseTraidPairs;
        }
    }
}
