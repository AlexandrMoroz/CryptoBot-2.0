using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.TraidPairsModels
{
    public class BaseTraidPairField
    {
        public int Id { get; set; }
        public string MarketCurrency { get; set; }  
        public string BaseCurrency { get; set; }
        public string MarketCurrencyLong { get; set; }
        public string BaseCurrencyLong { get; set; }
        public decimal MinTradeSize { get; set; }
        public string MarketName { get; set; }
        public decimal  Ask { get; set; }
        public decimal Bid { get; set; }
        public bool IsActive { get; set; }
      
    }
    public class BaseTraidPairModel
    {
        public Dictionary<string, string> ExceptionSymbols { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, BaseTraidPairField> Pairs { get; set; } = new Dictionary<string, BaseTraidPairField>();
        public BaseTraidPairModel()
        {
            ExceptionSymbols.Add("XDOGE", "DOGE");
        } 
        public virtual BaseTraidPairModel ToBaseTraidPairs()
        {
            return this;
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
