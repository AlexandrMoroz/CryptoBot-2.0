using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{
    public class CoinexBuySellField
    {
        public string Type{ get; set; }
        public string Price { get; set; }
        public string OrderTime { get; set; }
        public string Quantity { get; set; }
    }
    public class CoinexOrderField
    {
        public List<CoinexBuySellField> SellOrders { get; set; }
        public List<CoinexBuySellField> BuyOrders { get; set; }

    }
    public class CoinexOrderModel:BaseOrderModel
    {
        public string success { get; set; }
        public CoinexOrderField result { get; set; }
        public override BaseOrderModel ToBaseOrderModel()
        {
            var baseOrder = new BaseOrderModel();
            foreach (var item in result.SellOrders)
            {
                var temp = ToDecimal(item.Price);
                if (baseOrder.asks.ContainsKey(temp))
                {
                    baseOrder.asks[temp] += ToDecimal(item.Quantity);
                    continue;
                }
                baseOrder.asks.Add(temp, ToDecimal(item.Quantity));

            }
            foreach (var item in result.BuyOrders)
            {
                var temp = ToDecimal(item.Price);
                if (baseOrder.bids.ContainsKey(temp))
                {
                    baseOrder.bids[temp] += ToDecimal(item.Quantity);
                    continue;
                }
                baseOrder.bids.Add(temp, ToDecimal(item.Quantity));
            }

            return baseOrder;
        }

        private decimal ToDecimal(string str)
        {

            return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
}
