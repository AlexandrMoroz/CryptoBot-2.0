using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{
    public class BitbayOrderModel: BaseOrderModel
    {
        public List<dynamic> bids { get; set; }
        public List<dynamic> asks { get; set; }
        public override BaseOrderModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrderModel();
            foreach (var item in asks)
            {
                decimal price = item[0];
                decimal quantity = item[1];
                baseOrder.asks.Add(price, quantity);

            }
            foreach (var item in bids)
            {
                decimal price = item[0];
                decimal quantity = item[1];
                baseOrder.bids.Add(price, quantity);

            }
            return baseOrder;
        }
    }
}
