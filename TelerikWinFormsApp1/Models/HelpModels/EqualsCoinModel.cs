using Cryptobot.Stocks;

namespace Cryptobot.Models.HelpModels
{
    public class EqualsCoinModel
    {
        public string Market { get; set; }
        public string MarketCoin { get; set; }
        public string BaseCoin { get; set; }
        public string secMarketCoin { get; set; }
        public string secBaseCoin { get; set; }
        public Stock FirstStock { get; set; }
        public Stock SecondStock { get; set; }
        public decimal FirstStockAsk { get; set; }
        public decimal FirstStockBid { get; set; }
        public decimal SecondStockAsk { get; set; }
        public decimal SecondStockBid { get; set; }
    }
}
