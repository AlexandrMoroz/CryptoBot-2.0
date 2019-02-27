using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.TickerModels
{
    public class GetTikersField
    {
        public string lowestAsk  { get; set; }
        public string highestBid { get; set; }
    }
    public class GetTikersModel: BaseTickerModel
    {
        public Dictionary<string, GetTikersField> tikers { get; set; }
        public GetTikersModel(Dictionary<string, GetTikersField> arg)
        {
            tikers = arg;
        }
        public override BaseTickerModel ToBaseTikerModel()
        {
            var baseModel = new BaseTickerModel();
            baseModel.Tikers = tikers.ToDictionary(x => x.Key, y => new BaseTickerField()
            {
                Ask = ToDecimal(y.Value.lowestAsk),
                Bid = ToDecimal(y.Value.highestBid)

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
