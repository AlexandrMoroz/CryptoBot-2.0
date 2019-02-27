using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWinFormsApp1.Models.HelpModels
{
    public class ArbitrationViewModel
    {
        public string Stock { get; set; }

        [DisplayName("First market pair")]
        public string FirstMmarket { get; set; }
        [DisplayName("Second market pair")]
        public string SecondMarket { get; set; }
        [DisplayName("Base coin pair")]
        public string ThirdMarket { get; set; }
        //BTC-USD

        public decimal CoinBuyQuantity { get; set; }
        public decimal CoinSellPrice { get; set; }
        public decimal BTCSellPrice { get; set; }
        public decimal Profit { get; set; }
    }
}
