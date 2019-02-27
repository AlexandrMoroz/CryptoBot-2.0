using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{
    public class QuantityRate
    {
        public decimal Quantity;
        public decimal Rate;
    }
    public class BittrexResult
    {
        public List<QuantityRate> buy;
        public List<QuantityRate> sell;
    }
    public class BittrexOrderModel : BaseOrderModel
    {

        public bool success;
        public string message;
        public BittrexResult result;

        public override BaseOrderModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrderModel()
            {
                asks = result.buy.ToDictionary(x => x.Rate, x => x.Quantity),
                bids = result.sell.ToDictionary(x => x.Rate, x => x.Quantity)
            };
            return baseOrder;
        }
    }
    public class BittrexOrdersModel : BaseOrdersModel
    {

        Dictionary<string, BaseOrderModel> Orders { get; set; }

        public BittrexOrdersModel(Dictionary<string, BaseOrderModel> arg)
        {
            Orders = arg;
        }
        public override BaseOrdersModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrdersModel();
            baseOrder.Orders = Orders.ToDictionary(x => ReversMarketName(x.Key), y => y.Value);
            return baseOrder;
        }

        private string ReversMarketName(string arg) {
            return arg.Split('-')[1] +'-'+ arg.Split('-')[0];
        }
    }
}
