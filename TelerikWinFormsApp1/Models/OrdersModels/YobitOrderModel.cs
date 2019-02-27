using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{
    public class YobitOrderField
    {
        public List<List<decimal>> asks { get; set; }
        public List<List<decimal>> bids { get; set; }
    }
    public class YobitOrderModel : BaseOrderModel
    {
        public Dictionary<string, YobitOrderField> order { get; set; }
        public override BaseOrderModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrderModel();
            baseOrder.asks = order.First().Value.asks.ToDictionary(x => x[0], y => y[1]);
            baseOrder.bids = order.First().Value.bids.ToDictionary(x => x[0], y => y[1]);
            return baseOrder;

        }
    }
    public class YobitAllOrderModel : BaseOrdersModel
    {
        public Dictionary<string, YobitOrderField> order { get; set; }
        public override BaseOrdersModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrdersModel();
            foreach (var item in order)
            {
                baseOrder.Orders.Add(item.Key, new BaseOrderModel()
                {
                    asks = item.Value.asks.ToDictionary(x => x[0], y => y[1]),
                    bids = item.Value.bids.ToDictionary(x => x[0], y => y[1])
                });

            }

            return baseOrder;

        }
    }
}
