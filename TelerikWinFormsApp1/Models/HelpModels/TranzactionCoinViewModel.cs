using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWinFormsApp1.Models.HelpModels
{
    class TranzactionCoinViewModel
    {
        public string Pair { get; set; }
        public string MarketCoin { get; set; }
        public string BaseCoin { get; set; }
        public string FirstStock { get; set; }
        public string SecondStock { get; set; }
        public decimal BuyBid { get; set; }
        public decimal SellAks { get; set; }
        public decimal Profit { get; set; }
    }
}
