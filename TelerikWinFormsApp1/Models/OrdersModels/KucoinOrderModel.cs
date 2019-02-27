using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{
    public class KucoinOrderField
    {
        public List<dynamic> SELL { get; set; }
        public List<dynamic> BUY { get; set; }
    }
    public class KucoinOrderModel : BaseOrderModel
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public KucoinOrderField data { get; set; }
        public override BaseOrderModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrderModel();
            foreach (var item in data.SELL)
            {
                var temp = ToDecimal(item[0].ToString());
                if (baseOrder.asks.ContainsKey(temp))
                {
                    baseOrder.asks[temp] += ToDecimal(item[1].ToString());
                    continue;
                }
                baseOrder.asks.Add(temp, ToDecimal(item[1].ToString()));

            }
            foreach (var item in data.BUY)
            {
                var temp = ToDecimal(item[0].ToString());
                if (baseOrder.bids.ContainsKey(temp))
                {
                    baseOrder.bids[temp] += ToDecimal(item[1].ToString());
                    continue;
                }
                baseOrder.bids.Add(temp, ToDecimal(item[1].ToString()));
            }

            return baseOrder;
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

