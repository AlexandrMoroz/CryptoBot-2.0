using System.Threading.Tasks;
using Cryptobot.Models.OrdersModels;
using Cryptobot.Models.TraidPairsModels;
using Cryptobot.Stocks;

namespace Cryptobot.Models.HelpModels
{
    public class ArbitrationModel
    {
        public Stock Stock { get; set; }
        public string MarketCoinFirstBaseCoin_MarketName { get; set; }
        // ETH-USD 
        public BaseTraidPairField MarketCoinFirstBaseCoin { get; set; }
        public string MarketCoinSecondBaseCoin_MarketName { get; set; }
        //ETH-BTC
        public BaseTraidPairField MarketCoinSecondBaseCoin { get; set; }
        public string BaseCoinSecondBaseCoin_MarketName { get; set; }
        //BTC-USD
        public BaseTraidPairField BaseCoinSecondBaseCoin { get; set; }
    }
}
