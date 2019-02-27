using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{
    public class ExmoOrderField
    {

        public string ask_quantity { get; set; }
        public string ask_amount { get; set; }
        public string ask_top { get; set; }
        public string bid_quantity { get; set; }
        public string bid_amount { get; set; }
        public string bid_top { get; set; }
        public List<dynamic> bid { get; set; }
        public List<dynamic> ask { get; set; }
    }
    public class ExmoOrderModel:BaseOrderModel
    {
        public Dictionary<string, ExmoOrderField> Pair { get; set; }
        public override BaseOrderModel ToBaseOrderModel()
        {
            BaseOrderModel baseOrder = new BaseOrderModel();
            
            foreach (var i in Pair.First().Value.ask)
            {
                decimal price = i[0];
                decimal count = i[1];
                baseOrder.asks.Add(price, count);
            }
            foreach (var i in Pair.First().Value.bid)
            {
                decimal price = i[0];
                decimal count = i[1];
                baseOrder.bids.Add(price, count);
            }


            return baseOrder;
        }
    }
    public class ExmoOrdersModel : BaseOrdersModel
    {
        public Dictionary<string, ExmoOrderField> Pair { get; set; }
        public override BaseOrdersModel ToBaseOrderModel()
        {
            BaseOrdersModel baseOrders = new BaseOrdersModel();

            foreach (var item in Pair)
            {
                BaseOrderModel baseOrder = new BaseOrderModel();
                foreach (var i in item.Value.ask)
                {
                    decimal price = i[0];
                    decimal count = i[1];
                    baseOrder.asks.Add(price, count);
                }
                foreach (var i in item.Value.bid)
                {
                    decimal price = i[0];
                    decimal count = i[1];
                    baseOrder.bids.Add(price, count);
                }
                baseOrders.Orders.Add(item.Key, baseOrder);
            }

            return baseOrders;
        }
    }
}
