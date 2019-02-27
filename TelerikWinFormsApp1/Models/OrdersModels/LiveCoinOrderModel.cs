using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{

    public class LiveCoinOrderModel : BaseOrderModel
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
        private decimal ToDecimal(string str)
        {

            return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint|System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }

    public class LiveCoinAllOrderModel : BaseOrdersModel
    {
        public  Dictionary<string, LiveCoinOrderModel> AllOrders { get; set; }
        public override BaseOrdersModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrdersModel()
            {
                Orders = AllOrders.ToDictionary(x => x.Key, y => y.Value.ToBaseOrderModel())
            };
            return baseOrder;
        }
    }
}
