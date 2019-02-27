using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Models.TickerModels;

namespace Cryptobot.Models.TraidPairsModels
{
    public class YobitTraidPairsField
    {
        public decimal fee { get; set; }
        public decimal fee_buyer { get; set; }
        public decimal fee_seller { get; set; }
        public decimal min_amount { get; set; }
    }
    public class YobitTraidPairsModel : BaseTraidPairModel
    {
        public Dictionary<string, YobitTraidPairsField> pairs { get; set; }
        private Dictionary<string, string> data { get; set; }
        public YobitTraidPairsModel()
        {
            data = GetSymbolAndName();
        }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var baseTraidPairs = new BaseTraidPairModel();
            foreach (var item in pairs)
            {
                var MarketCurrency = item.Key.Split('_')[0].ToUpper();
                var BaseCurrency = item.Key.Split('_')[1].ToUpper();
                var MarketName = MarketCurrency + "-" + BaseCurrency;
                var marketLong = data.FirstOrDefault(x => x.Key == MarketCurrency);
                var baseLong = data.FirstOrDefault(x => x.Key == BaseCurrency);
                if (marketLong.Key != null && baseLong.Value != null)
                {
                    if (!baseTraidPairs.Pairs.ContainsKey(MarketName))
                    {
                        baseTraidPairs.Pairs.Add(MarketName, new BaseTraidPairField()
                        {
                            MarketCurrency = MarketCurrency,
                            BaseCurrency = BaseCurrency,
                            MarketCurrencyLong = marketLong.Value,
                            BaseCurrencyLong = baseLong.Value,
                            MarketName = MarketName,
                            MinTradeSize = item.Value.min_amount
                        });
                    }
                }
            }
            return baseTraidPairs;
        }
        private Dictionary<string, string> GetSymbolAndName()
        {
            var data = new Dictionary<string, string>();
            var url = "https://yobit.net/ru/coinsinfo";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var table = doc.DocumentNode.SelectNodes("//table[@id='downloads_table']/tbody/tr");
            foreach (var item in table)
            {
                var symbol = item.ChildNodes[1].InnerText;
                var name = item.ChildNodes[3].InnerText;
                data.Add(symbol, name);
            }
            return data;
        }
    }
    public class YobitTraidPairsMainModel : BaseTraidPairModel
    {
        public Dictionary<string, YobiTickerField> tiker { get; set; }
        public BaseTraidPairModel Pairs { get; set; }
        public YobitTraidPairsMainModel(Dictionary<string, YobiTickerField> arg, BaseTraidPairModel pairs)
        {
            tiker = arg;
            Pairs = pairs;
        }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            foreach (var item in Pairs.Pairs)
            {
                var MarketName = item.Value.MarketCurrency + "-" + item.Value.BaseCurrency;
                var tick = tiker.FirstOrDefault(x => x.Key == item.Value.MarketCurrency + '-' + item.Value.BaseCurrency);
                if (tick.Key != null)
                {
                    var value = Pairs.Pairs[MarketName];
                    Pairs.Pairs[MarketName] = new BaseTraidPairField()
                    {
                        MarketCurrency = value.MarketCurrency,
                        BaseCurrency = value.BaseCurrency,
                        MarketCurrencyLong = value.MarketCurrencyLong,
                        BaseCurrencyLong = value.BaseCurrencyLong,
                        MarketName = value.MarketName,
                        MinTradeSize = value.MinTradeSize,
                        Ask = tick.Value.sell,
                        Bid = tick.Value.buy
                    };
                }
            }

            return Pairs;
        }
    }
}
