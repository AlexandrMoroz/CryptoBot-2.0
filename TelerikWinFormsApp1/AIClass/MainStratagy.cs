using System.Collections.Generic;
using System.ComponentModel;
using Cryptobot.Stocks;
namespace Cryptobot
{
    public class MainStrategy
    {

        public string MarketName = "";
        public Stock BuyStockEX;
        public Stock SellStockEX;
        public Dictionary<decimal, decimal> StrategyBuy = new Dictionary<decimal, decimal>();
        public Dictionary<decimal, decimal> StrategySell = new Dictionary<decimal, decimal>();

    }
   
    public class StrategyField
    {
        //1 - price, 2 - count
        public Dictionary<decimal, decimal> Data = new Dictionary<decimal, decimal>();

        public override string ToString()
        {
            string temp = "";

            foreach (var i in Data)
            {
                temp += "цена: " + i.Key + " Количество: " + i.Value + '\n';
            }
            return temp;
        }
    }
}
