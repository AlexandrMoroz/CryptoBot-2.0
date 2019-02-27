using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.OrdersModels
{
    public class CryptopiaOrderField
    {
        public int TradePairId;
        public decimal Price;
        public decimal Volume;
        public decimal Total;
    }
    public class BuySellData
    {

        public List<CryptopiaOrderField> Buy;
        public List<CryptopiaOrderField> Sell;

    }
    class CryptopiaOrderModel:BaseOrderModel
    {
        public bool Success;
        public string Error;
        public BuySellData Data;
        public override BaseOrderModel ToBaseOrderModel()
        {
            return new BaseOrderModel()
            {
                asks = Data.Buy.ToDictionary(x => x.Price, y => y.Volume),
                bids = Data.Sell.ToDictionary(x => x.Price, y => y.Volume)
            };   
        }
    }
}
