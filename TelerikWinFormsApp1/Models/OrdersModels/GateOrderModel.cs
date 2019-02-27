using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{

    public  class GateOrderModel: BaseOrderModel
    {
        public List<dynamic> asks { get; set; }
        public List<dynamic> bids { get; set; }
        public bool result { get; set; }
        public override BaseOrderModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrderModel();

            foreach (var i in asks)
            {
                decimal price = i[0];
                decimal count = i[1];
                baseOrder.asks.Add(price, count);
            }
            foreach (var i in bids)
            {
                decimal price = i[0];
                decimal count = i[1];
                baseOrder.bids.Add(price, count);
            }
            return baseOrder;
        }
    }
    //public class GateOrdersModel: BaseOrdersModel
    //{
    //    public override BaseOrderModel ToBaseOrderModel()
    //    {
    //        var baseOrder = new BaseOrderModel();

    //        foreach (var i in asks)
    //        {
    //            decimal price = i[0];
    //            decimal count = i[1];
    //            baseOrder.asks.Add(price, count);
    //        }
    //        foreach (var i in bids)
    //        {
    //            decimal price = i[0];
    //            decimal count = i[1];
    //            baseOrder.bids.Add(price, count);
    //        }
    //        return baseOrder;
    //    }
    //}
}
