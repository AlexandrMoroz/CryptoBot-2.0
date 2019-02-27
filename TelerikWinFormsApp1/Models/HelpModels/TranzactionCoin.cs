using Cryptobot.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWinFormsApp1.Models.HelpModels
{
    public class TranzactionCoinModel
    {
        public string PairName{ get; set; }
        public string MarketCoin { get; set; }
        public string BaseCoin { get; set; }
        public Stock FirstStock { get; set; }
        public Stock SecondStock { get; set; }
        public decimal BuyBid { get; set; }
        public decimal SellAks { get; set; }
        public decimal Profit { get; set; }
    }
}
