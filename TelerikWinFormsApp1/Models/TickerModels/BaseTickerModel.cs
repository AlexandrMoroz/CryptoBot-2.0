using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.TickerModels
{
    public class BaseTickerField
    {
        public decimal Ask { get; set; }
        public decimal Bid { get; set; }
    }
    public class BaseTickerModel
    {
        public Dictionary<string, BaseTickerField> Tikers { get; set; }
        public BaseTickerModel()
        {
            Tikers = new Dictionary<string, BaseTickerField>();
        }
        public virtual BaseTickerModel ToBaseTikerModel()
        {
            return this;
        }
    }
}
