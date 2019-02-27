using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.TickerModels
{
    public class CoinexTickerField
    {
        public string MarketID { get; set; }
        public string BidPrice { get; set; }
        public string AskPrice { get; set; }

    }
    public  class CoinexTickerModel:BaseTickerModel
    {
        public List<CoinexTickerField> result { get; set; }
        public override BaseTickerModel ToBaseTikerModel()
        {
            var baseModel = new BaseTickerModel();
            baseModel.Tikers = result.ToDictionary(x => x.MarketID, y => new BaseTickerField()
            {
                Ask = ToDecimal(y.AskPrice),
                Bid= ToDecimal(y.BidPrice)

            });
            return baseModel;
        }
        private decimal ToDecimal(string str)
        {
            return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
}
