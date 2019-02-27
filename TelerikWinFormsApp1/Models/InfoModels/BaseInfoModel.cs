using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.InfoModels
{
    public class BaseInfoField
    {
        public decimal WithdrawFee { get; set; }
        public bool Status { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public bool WalletStatus { get; set; }
        public decimal Difficulty { get; set; }
        public decimal MinDepositAmount { get; set; }
        public decimal MinWithdrawAmount { get; set; }
        public decimal MinOrderAmount { get; set; }
    }
    public class BaseInfoModel
    {
        public Dictionary<string, BaseInfoField> CoinsInfo {get;set;} = new Dictionary<string, BaseInfoField>();
        public Dictionary<string, string> ExceptionNames { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ExceptionSymbols { get; set; } = new Dictionary<string, string>();
        public BaseInfoModel()
        {
            ExceptionSymbols.Add("XDOGE", "DOGE");

            ExceptionNames.Add("XDOGE", "DOGECOIN");
            ExceptionNames.Add("USD", "USD");
            ExceptionNames.Add("EUR", "EUR");
            ExceptionNames.Add("ZRX", "0x");
            ExceptionNames.Add("BCH", "BITCOIN CASH");
            ExceptionNames.Add("RLC", "IEX.EC");
            ExceptionNames.Add("PAY", "TENX");
            ExceptionNames.Add("GAME", "GAME CREDITS");
            ExceptionNames.Add("BTG", "BITCOIN GOLD");
            ExceptionNames.Add("USDT", "USDT");
            ExceptionNames.Add("BTC", "BITCOIN");
            ExceptionNames.Add("CNYX ", "CNYX");
            ExceptionNames.Add("ETH", "ETHEREUM ");
            ExceptionNames.Add("QTUM", "QTUM");
            ExceptionNames.Add("SNET", "SNETWORK");
            ExceptionNames.Add("BAT", "BASIC ATTENTION TOKEN");
            ExceptionNames.Add("LTC", "LITECOIN");
            ExceptionNames.Add("SNT", "STATUS NETWORK TOKEN");
            ExceptionNames.Add("TX", "TRANSFERCOIN");
            ExceptionNames.Add("XRP", "RIPPLE");
        }
        public virtual BaseInfoModel ToBaseInfoModel() {
            return this;
        }
        public string CheakName(string coinSymbol, string coinName)
        {
            if (ExceptionNames.ContainsKey(coinSymbol))
            {
                return ExceptionNames[coinSymbol];
            }
            return coinName;
        }
        public string CheakSymbol(string coinSymbol)
        {
            if (ExceptionSymbols.ContainsKey(coinSymbol))
            {
                return ExceptionSymbols[coinSymbol];
            }
            return coinSymbol;
        }
    }
}
