using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.TickerModels
{
    public class ExmoTikerField
    {
        public string buy_price { get; set; }
        public string sell_price { get; set; }
    }
    public class ExmoTikerModel:BaseTickerModel
    {
        public Dictionary<string, ExmoTikerField> tiker { get; set; } 
        public override BaseTickerModel ToBaseTikerModel()
        {
            var baseModel = new BaseTickerModel();
            baseModel.Tikers = tiker.ToDictionary(x => x.Key, y => new BaseTickerField()
            {
                Ask = ToDecimal(y.Value.sell_price),
                Bid = ToDecimal(y.Value.buy_price)

            });
            return baseModel;
        }
        private decimal ToDecimal(string str)
        {

            if (str.Contains('E') || str.Contains('e') || str.Contains('.'))
            {
                return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float | NumberStyles.Number, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            else
            {
                return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float | NumberStyles.Number, new CultureInfo("fr-FR"));
            }
        }
    }
}
