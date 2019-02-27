using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Models.InfoModels;
using Cryptobot.Stocks;

namespace Cryptobot.Models.TraidPairsModels

{public class CryptoBridgePairsField {
        public string id { get; set; }
        public string last { get; set; }
        public string volume { get; set; }
        public string ask { get; set; }
        public string bid { get; set; }
        public double percentChange { get; set; }
        public bool restricted { get; set; }
    }
    public class CryptoBridgePairsModel:BaseTraidPairModel
    {
        private Task<BaseInfoModel> info = new CryptoBridgeInfo().GetInfoAsync();
        public List<CryptoBridgePairsField> Pairs { get; set; }
        private Dictionary<string, string> StockBaseCoins { get; set; } = new Dictionary<string, string>();
        public CryptoBridgePairsModel(List<CryptoBridgePairsField> arg)
        {
            Pairs = arg;
        }
        public override BaseTraidPairModel ToBaseTraidPairs()
        {
            var basePairsModel = new BaseTraidPairModel();
            var info = this.info.Result;
            foreach (var item in Pairs)
            {
                var marketName = CheakSymbol(item.id.Split('_')[0]) + '-' + CheakSymbol(item.id.Split('_')[1]);
                var marketCurrencyLong = info.CoinsInfo.FirstOrDefault(x => x.Key == item.id.Split('_')[0]);
                var baseCurrencyLong = info.CoinsInfo.FirstOrDefault(x => x.Key == item.id.Split('_')[1]);
                if (marketCurrencyLong.Key!=null && baseCurrencyLong.Key != null)
                {
                    var marketLong = marketCurrencyLong.Value.Name.ToUpper();
                    var baseLong = baseCurrencyLong.Value.Name.ToUpper();
                    basePairsModel.Pairs.Add(marketName,
                        new BaseTraidPairField()
                        {
                            MarketCurrency = CheakSymbol(item.id.Split('_')[0]),
                            BaseCurrency = CheakSymbol(item.id.Split('_')[1]),
                            MarketCurrencyLong = marketLong,
                            BaseCurrencyLong = baseLong,
                            Ask = ToDecimal(item.ask),
                            Bid = ToDecimal(item.bid)
                        });
                }
            }
            return basePairsModel;
        }

        private decimal ToDecimal(string str)
        {

            return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
}
