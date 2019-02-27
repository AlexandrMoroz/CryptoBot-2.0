using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{
    public class BaseOrderModel
    {
        public Dictionary<decimal, decimal> bids { get; set; }
        public Dictionary<decimal, decimal> asks { get; set; }
        public BaseOrderModel()
        {
            bids = new Dictionary<decimal, decimal>();
            asks = new Dictionary<decimal, decimal>();
        }
        public virtual BaseOrderModel ToBaseOrderModel()
        {
            return this;
        }
    }
    public class BaseOrdersModel
    {
        public Dictionary<string, BaseOrderModel> Orders { get; set; }
        public BaseOrdersModel()
        {
            Orders = new Dictionary<string, BaseOrderModel>();
        }

        public virtual BaseOrdersModel ToBaseOrderModel()
        {
            return this;
        }
    }
}
