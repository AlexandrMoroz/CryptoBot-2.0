using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{
    public class PoloniexOrderField
    {
        public decimal Key { get; set; }
        public decimal Value { get; set; }
    }
    public class PoloniexOrderModel : BaseOrderModel
    {
        public List<List<string>> asks { get; set; }
        public List<List<string>> bids { get; set; }

        public override BaseOrderModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrderModel();
            foreach (var item in asks)
            {
                var temp = ToDecimal(item[0]);
                if (baseOrder.asks.ContainsKey(temp))
                {
                    baseOrder.asks[temp] += ToDecimal(item[1]);
                    continue;
                }
                baseOrder.asks.Add(temp, ToDecimal(item[1]));

            }
            foreach (var item in bids)
            {
                var temp = ToDecimal(item[0]);
                if (baseOrder.bids.ContainsKey(temp))
                {
                    baseOrder.bids[temp] += ToDecimal(item[1]);
                    continue;
                }
                baseOrder.bids.Add(temp, ToDecimal(item[1]));
            }

            return baseOrder;
        }
        public decimal ToDecimal(string str)
        {

            return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
    public class PoloniexAllOrderModel : BaseOrdersModel
    {
        public Dictionary<string, PoloniexOrderModel> Orders { get; set; }
        public PoloniexAllOrderModel(Dictionary<string, PoloniexOrderModel> arg)
        {
            Orders = arg;
        }
        public override BaseOrdersModel ToBaseOrderModel()
        {
            return new BaseOrdersModel() { Orders = Orders.ToDictionary(x => x.Key, y => y.Value.ToBaseOrderModel()) };

        }
    }
}
