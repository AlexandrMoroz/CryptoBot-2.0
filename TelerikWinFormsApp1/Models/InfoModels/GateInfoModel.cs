using System.Collections.Generic;
using System.Linq;
using Cryptobot.Models.TraidPairsModels;
using Cryptobot.Stocks;

namespace Cryptobot.Models.InfoModels
{
    public class GateInfoField
    {
        public short delisted { get; set; }
        public short withdaw_disable { get; set; }
        public short withdraw_delayed { get; set; }
        public short deposit_disabled { get; set; }
        public short trade_disabled { get; set; }
    }
    public class GateInfoModel : BaseInfoModel
    {
        public string result { get; set; }
        public List<Dictionary<string, GateInfoField>> coins { get; set; }
        private Dictionary<string, string> StockBaseCoins { get; set; }
        public GateInfoModel()
        {
            StockBaseCoins = new Dictionary<string, string>();

        }
        public override BaseInfoModel ToBaseInfoModel()
        {
            var baseInfo = new BaseInfoModel();
            var pairs = new GatePairs().GetTraidPairsAsync().Result;

            foreach (var item in coins)
            {
                if (!baseInfo.CoinsInfo.ContainsKey(item.First().Key))
                {
                    baseInfo.CoinsInfo.Add(item.First().Key, new BaseInfoField()
                    {
                        Symbol = item.First().Key,
                        Name = CheakName(pairs, item),
                        WalletStatus = CheakWallet(item.First().Value),
                        Status = CheakWallet(item.First().Value)
                    });
                }

            }

            return baseInfo;
        }
        private string CheakName(BaseTraidPairModel pairs, Dictionary<string, GateInfoField> info)
        {
            var coinSymbol = info.First().Key;
            var pair = pairs.Pairs.FirstOrDefault(j => j.Value.BaseCurrency==coinSymbol|| j.Value.MarketCurrency == coinSymbol);
            if (pair.Value != null)
            {
                if (pair.Value.BaseCurrency.Contains(coinSymbol))
                {
                    if (StockBaseCoins.ContainsKey(coinSymbol))
                    {
                        return StockBaseCoins[coinSymbol];
                    }
                    return pair.Value.BaseCurrencyLong;
                }
                if (pair.Value.MarketCurrency.Contains(coinSymbol))
                {
                    if (StockBaseCoins.ContainsKey(coinSymbol))
                    {
                        return StockBaseCoins[coinSymbol];
                    }
                    return pair.Value.MarketCurrencyLong;
                }
            }
            return "";
        }

        private bool CheakWallet(GateInfoField arg)
        {
            List<bool> temp = new List<bool>();
            temp.Add(arg.delisted == 1 ? false : true);
            temp.Add(arg.withdaw_disable == 1 ? false : true);
            temp.Add(arg.withdraw_delayed == 1 ? false : true);
            temp.Add(arg.deposit_disabled == 1 ? false : true);
            temp.Add(arg.trade_disabled == 1 ? false : true);

            return !temp.Any(x => x == false);

        }
    }
}
