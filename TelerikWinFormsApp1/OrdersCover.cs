using Cryptobot.Models.HelpModels;
using Cryptobot.Models.InfoModels;
using Cryptobot.Models.OrdersModels;
using Cryptobot.Models.TraidPairsModels;
using Cryptobot.Stocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptobot
{
    public class ArbitrationByOneStock
    {
        public List<ArbitrationModel> models { get; set; }
        public ArbitrationByOneStock(Stock stockArg)
        {
            models = GetPairsCanDoArbitration(stockArg);
        }
        private List<ArbitrationModel> GetPairsCanDoArbitration(Stock stock)
        {
            var allOrders = stock.TraidPairs.GetTraidPairsAsync().Result;
            var BaseCoins = new List<string>();
            foreach (var item in allOrders.Pairs)
            {
                if (!BaseCoins.Contains(item.Value.BaseCurrency))
                {
                    BaseCoins.Add(item.Value.BaseCurrency);
                }
            }
            // 1 выборка первых значений
            // 2 проверить есть ли торговая пара из двух основный монет (BTC-USD, BTC-USDT)
            // 3 выборка вторых значений и сравнение с первыми. результат записать в масив
            var Models = new List<ArbitrationModel>();
            for (int i = 0; i < BaseCoins.Count(); i++)
            {
                var FirstPairs = allOrders.Pairs.Where(x => x.Value.BaseCurrency == BaseCoins[i]);
                for (int j = i + 1; j < BaseCoins.Count() - 1; j++)
                {
                    var CheakTwoBaseCoinsForPairs = allOrders.Pairs.FirstOrDefault(x => x.Value.BaseCurrency == BaseCoins[i] && x.Value.MarketCurrency == BaseCoins[j]
                                             || x.Value.BaseCurrency == BaseCoins[j] && x.Value.MarketCurrency == BaseCoins[i]);
                    if (CheakTwoBaseCoinsForPairs.Key != null)
                    {
                        var SecondPairs = allOrders.Pairs.Where(x => x.Value.BaseCurrency == BaseCoins[j]);
                        var FirstStockSelectedPairs = FirstPairs.Where(x => SecondPairs.FirstOrDefault(y => y.Value.MarketCurrency == x.Value.MarketCurrency).Key != null);
                        var SecondStockSelectedPairs = SecondPairs.Where(x => FirstPairs.FirstOrDefault(y => y.Value.MarketCurrency == x.Value.MarketCurrency).Key != null);
                        foreach (var item in FirstStockSelectedPairs)
                        {
                            var model = new ArbitrationModel();
                            model.Stock = stock;
                            var FirstPair = item.Key;
                            model.MarketCoinFirstBaseCoin_MarketName = FirstPair;

                            var SecondPair = SecondStockSelectedPairs.First(x => x.Value.MarketCurrency == item.Value.MarketCurrency);
                            model.MarketCoinSecondBaseCoin_MarketName = SecondPair.Key;

                            model.MarketCoinFirstBaseCoin = item.Value;
                            model.MarketCoinSecondBaseCoin = SecondPair.Value;

                            var TwoBaseCoins = CheakTwoBaseCoinsForPairs;
                            model.BaseCoinSecondBaseCoin_MarketName = TwoBaseCoins.Key;
                            model.BaseCoinSecondBaseCoin = TwoBaseCoins.Value;
                            if (!Models.Contains(model))
                            {
                                Models.Add(model);
                            }
                        }

                    }
                }
            }
            return Models;
        }
    }
    public class OrdersCover2Stock
    {
        public Stock stock;
        public Task<BaseOrdersModel> order;
        public OrdersCover2Stock(Stock stockArg, List<KeyValuePair<string, string>> pairs)
        {
            stock = stockArg;
            order = stockArg.Orders.GetOrdersAsync(pairs);

        }

    }
  

}