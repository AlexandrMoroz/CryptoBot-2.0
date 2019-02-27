using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWinFormsApp1.Models.HelpModels
{
    public class EqualsCoinViewModel
    {
        public string Pair { get; set; }
        public string MarketCoin { get; set; }
        public string BaseCoin { get; set; }
        [JsonIgnore]
        public string SecondMarketCoin { get; set; }
        [JsonIgnore]
        public string SecondBaseCoin { get; set; }
        [JsonIgnore]
        public string FirstStock { get; set; }
        [JsonIgnore]
        public string SecondStock { get; set; }
        [JsonIgnore]
        public string Ask { get; set; }
        [JsonIgnore]
        public string Bid { get; set; }
    }
}
