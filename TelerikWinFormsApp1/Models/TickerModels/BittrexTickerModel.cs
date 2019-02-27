using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.TickerModels
{
    public class BittrexTickerField
    {
        public string MarketName { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
    public class BittrexTickerModel:BaseTickerModel
    {
        public List<BittrexTickerField> result { get; set; }
        public override BaseTickerModel ToBaseTikerModel()
        {
            var baseModel = new BaseTickerModel();
            baseModel.Tikers = result.ToDictionary(x => x.MarketName, y => new BaseTickerField()
            {
                Ask = y.Ask,
                Bid = y.Bid

            });
            return baseModel;
        }
    }
}
