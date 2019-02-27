using Cryptobot.Models.HelpModels;
using Cryptobot.Models.InfoModels;
using Cryptobot.Models.OrdersModels;
using Cryptobot.Models.TraidPairsModels;
using Cryptobot.Stocks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelerikWinFormsApp1.Models.HelpModels;

namespace Cryptobot.AIClass
{
    public class TranzactionCoinCompare
    {
        private List<EqualsCoinModel> EqualsCoins { get; set; }
        public TranzactionCoinCompare(List<EqualsCoinModel> equalsCoins)
        {
            EqualsCoins = equalsCoins;
        }
        public List<TranzactionCoinModel> TranzactionCompare()
        {
            var Models = new List<TranzactionCoinModel>();
            foreach (var EqualsCoin in EqualsCoins)
            {
                if (IsProfit(EqualsCoin.FirstStockBid, EqualsCoin.SecondStockAsk))
                {
                    Models.Add(new TranzactionCoinModel()
                    {
                        PairName = EqualsCoin.Market,
                        MarketCoin = EqualsCoin.MarketCoin,
                        BaseCoin = EqualsCoin.BaseCoin,
                        FirstStock = EqualsCoin.FirstStock,
                        SecondStock = EqualsCoin.SecondStock,
                        BuyBid = EqualsCoin.FirstStockBid,
                        SellAks = EqualsCoin.SecondStockAsk,
                        Profit = GetProfit(EqualsCoin.FirstStockBid ,EqualsCoin.SecondStockAsk)
                    });
                }
                else if (IsProfit(EqualsCoin.SecondStockBid, EqualsCoin.FirstStockAsk))
                {
                    Models.Add(new TranzactionCoinModel()
                    {
                        PairName = EqualsCoin.Market,
                        MarketCoin = EqualsCoin.MarketCoin,
                        BaseCoin = EqualsCoin.BaseCoin,
                        FirstStock = EqualsCoin.SecondStock,
                        SecondStock = EqualsCoin.FirstStock,
                        BuyBid = EqualsCoin.SecondStockBid,
                        SellAks = EqualsCoin.FirstStockAsk,
                        Profit= GetProfit(EqualsCoin.SecondStockBid, EqualsCoin.FirstStockAsk)
                    });
                }
            }
            return Models;
        }
        private decimal GetProfit(decimal bid, decimal ask )
        {
            var profit = bid - ask;
            profit -= profit * 0.005m;
            return profit;
        }
        public static List<MainStrategy> CoinCompare(BaseOrdersModel arg1, BaseOrdersModel arg2, Stock mainStockEx, Stock secoondStockEx)
        {
            if (arg1 == null || arg2 == null)
            {
                return new List<MainStrategy>();
            }
            var main = arg1.Orders.Where(d => arg2.Orders.ContainsKey(d.Key)).ToDictionary(x => x.Key, y => new BaseOrderModel()
            {
                asks = y.Value.asks.Count != 0 ? y.Value.asks.Take(30).ToDictionary(c => c.Key, v => v.Value) : y.Value.asks,
                bids = y.Value.bids.Count != 0 ? y.Value.bids.Take(30).ToDictionary(c => c.Key, v => v.Value) : y.Value.bids
            });
            var second = arg2.Orders.Where(d => arg1.Orders.ContainsKey(d.Key)).ToDictionary(x => x.Key, y => new BaseOrderModel()
            {
                asks = y.Value.asks.Count != 0 ? y.Value.asks.Take(30).ToDictionary(c => c.Key, v => v.Value) : y.Value.asks,
                bids = y.Value.bids.Count != 0 ? y.Value.bids.Take(30).ToDictionary(c => c.Key, v => v.Value) : y.Value.bids
            });
            #region сравнение валют и запись из в стратегию

            List<MainStrategy> MainSt = new List<MainStrategy>();

            foreach (KeyValuePair<string, BaseOrderModel> i in main)
            {
                BaseOrderModel temp;

                if (second.ContainsKey(i.Key))
                {
                    temp = second[i.Key];
                }
                else
                {
                    continue;
                }
                if (temp.asks.Count == 0)
                {
                    continue;
                }
                if (temp.bids.Count == 0)
                {
                    continue;
                }
                if (i.Value.asks.Count == 0)
                {
                    continue;
                }
                if (i.Value.bids.Count == 0)
                {
                    continue;
                }
                if (IsProfit(temp.bids.First().Key, i.Value.asks.First().Key))
                {
                    MainStrategy TempSt = new MainStrategy();
                    TempSt.MarketName = i.Key;
                    TempSt.BuyStockEX = mainStockEx;
                    TempSt.SellStockEX = secoondStockEx;
                    if (temp.asks.Count == 0)
                    {
                        continue;
                    }
                    if (temp.bids.Count == 0)
                    {
                        continue;
                    }
                    if (i.Value.asks.Count == 0)
                    {
                        continue;
                    }
                    if (i.Value.bids.Count == 0)
                    {
                        continue;
                    }
                    //buy on [livecoin] main sell on  [poloniex] several
                    while (IsProfit(temp.bids.First().Key, i.Value.asks.First().Key))
                    {

                        if (i.Value.asks.First().Value > temp.bids.First().Value)
                        {
                            //записать в стратегию значение цены и количество в цикле пока профит
                            if (TempSt.StrategyBuy.ContainsKey(i.Value.asks.First().Key))
                            {
                                TempSt.StrategyBuy[i.Value.asks.First().Key] += temp.bids.First().Value;
                            }
                            else
                            {
                                TempSt.StrategyBuy.Add(i.Value.asks.First().Key, temp.bids.First().Value);
                            }
                            if (TempSt.StrategySell.ContainsKey(temp.bids.First().Key))
                            {
                                TempSt.StrategySell[temp.bids.First().Key] += temp.bids.First().Value;
                            }
                            else
                            {
                                TempSt.StrategySell.Add(temp.bids.First().Key, temp.bids.First().Value);
                            }

                            //отнять в списках большее значение
                            i.Value.asks[i.Value.asks.First().Key] -= temp.bids.First().Value;
                            //удалить из списков меньшее значение
                            temp.bids.Remove(temp.bids.First().Key);

                        }
                        else if (i.Value.asks.First().Value < temp.bids.First().Value)
                        {
                            //записать в стратегию значение цены и количество
                            if (TempSt.StrategyBuy.ContainsKey(i.Value.asks.First().Key))
                            {
                                TempSt.StrategyBuy[i.Value.asks.First().Key] += i.Value.asks.First().Value;
                            }
                            else
                            {
                                TempSt.StrategyBuy.Add(i.Value.asks.First().Key, i.Value.asks.First().Value);
                            }
                            if (TempSt.StrategySell.ContainsKey(temp.bids.First().Key))
                            {
                                TempSt.StrategySell[temp.bids.First().Key] += i.Value.asks.First().Value;
                            }
                            else
                            {
                                TempSt.StrategySell.Add(temp.bids.First().Key, i.Value.asks.First().Value);
                            }
                            //отнять в списках большее значение
                            temp.bids[temp.bids.First().Key] -= i.Value.asks.First().Value;
                            //удалить из списков меньшее значение
                            i.Value.asks.Remove(i.Value.asks.First().Key);
                        }
                        if (temp.asks.Count == 0)
                        {
                            break;
                        }
                        if (temp.bids.Count == 0)
                        {
                            break;
                        }
                        if (i.Value.asks.Count == 0)
                        {
                            break;
                        }
                        if (i.Value.bids.Count == 0)
                        {
                            break;
                        }
                        //равные значения будут нужны для возврата валюты 
                        //else if (temp.bids.First()[1] == i.Value.asks.First()[1])
                        //{
                        //    //записать в стратегию значение цены и количество
                        //    MainSt.StrategyBuy.Data[temp.bids.First()[0]] += i.Value.asks.First()[1];
                        //    MainSt.StrategySell.Data[i.Value.asks.First()[0]] += i.Value.asks.First()[1];

                        //    //удалить из списков меньшее значение
                        //    temp.bids.First().Clear();
                        //    i.Value.asks.First().Clear();
                        //}

                    }
                    MainSt.Add(TempSt);
                }
                else if (IsProfit(i.Value.bids.First().Key, temp.asks.First().Key))
                {
                    //buy on [poloniex] main sell on [livecoin] several
                    MainStrategy TempSt = new MainStrategy();
                    TempSt.MarketName = i.Key;
                    TempSt.BuyStockEX = secoondStockEx;
                    TempSt.SellStockEX = mainStockEx;

                    //buy on [livecoin] main sell on  [poloniex] several
                    while (IsProfit(i.Value.bids.First().Key, temp.asks.First().Key))
                    {

                        if (temp.asks.First().Value > i.Value.bids.First().Value)
                        {
                            //записать в стратегию значение цены и количество в цикле пока профит
                            if (TempSt.StrategyBuy.ContainsKey(temp.asks.First().Key))
                            {
                                TempSt.StrategyBuy[temp.asks.First().Key] += i.Value.bids.First().Value;
                            }
                            else
                            {
                                TempSt.StrategyBuy.Add(temp.asks.First().Key, i.Value.bids.First().Value);
                            }
                            if (TempSt.StrategySell.ContainsKey(i.Value.bids.First().Key))
                            {
                                TempSt.StrategySell[i.Value.bids.First().Key] += i.Value.bids.First().Value;
                            }
                            else
                            {
                                TempSt.StrategySell.Add(i.Value.bids.First().Key, i.Value.bids.First().Value);
                            }

                            //отнять в списках большее значение
                            temp.asks[temp.asks.First().Key] -= -i.Value.bids.First().Value;
                            //удалить из списков меньшее значение
                            i.Value.bids.Remove(i.Value.bids.First().Key);


                        }
                        else if (temp.asks.First().Value < i.Value.bids.First().Value)
                        {
                            //записать в стратегию значение цены и количество
                            if (TempSt.StrategyBuy.ContainsKey(temp.asks.First().Key))
                            {
                                TempSt.StrategyBuy[temp.asks.First().Key] += temp.asks.First().Value;
                            }
                            else
                            {
                                TempSt.StrategyBuy.Add(temp.asks.First().Key, temp.asks.First().Value);
                            }
                            if (TempSt.StrategySell.ContainsKey(i.Value.bids.First().Key))
                            {
                                TempSt.StrategySell[i.Value.bids.First().Key] += temp.asks.First().Value;
                            }
                            else
                            {
                                TempSt.StrategySell.Add(i.Value.bids.First().Key, temp.asks.First().Value);
                            }
                            //отнять в списках большее значение
                            i.Value.bids[i.Value.bids.First().Key] -= temp.asks.First().Value;
                            //удалить из списков меньшее значение
                            temp.asks.Remove(temp.asks.First().Key);
                        }
                        if (temp.asks.Count == 0)
                        {
                            break;
                        }
                        if (temp.bids.Count == 0)
                        {
                            break;
                        }
                        if (i.Value.asks.Count == 0)
                        {
                            break;
                        }
                        if (i.Value.bids.Count == 0)
                        {
                            break;
                        }
                    }
                    MainSt.Add(TempSt);
                }

            }
            return MainSt;
            #endregion
        }
        public static MainStrategy CoinCompare(BaseOrderModel arg1, BaseOrderModel arg2, Stock mainStockEx, Stock secoondStockEx, string MarketName)
        {
            if (arg1 == null || arg2 == null)
            {
                return null;
            }
            BaseOrderModel main = new BaseOrderModel()
            {
                asks = arg2.asks.Take(30).ToDictionary(c => c.Key, v => v.Value),
                bids = arg2.bids.Take(30).ToDictionary(c => c.Key, v => v.Value)
            };
            BaseOrderModel second = new BaseOrderModel()
            {
                asks = arg1.asks.Take(30).ToDictionary(c => c.Key, v => v.Value),
                bids = arg1.bids.Take(30).ToDictionary(c => c.Key, v => v.Value)
            };
            #region сравнение валют и запись из в стратегию

            if (IsProfit(second.bids.First().Key, main.asks.First().Key))
            {
                MainStrategy TempSt = new MainStrategy();
                TempSt.MarketName = MarketName;
                TempSt.BuyStockEX = mainStockEx;
                TempSt.SellStockEX = secoondStockEx;

                //buy on [livecoin] main sell on  [poloniex] several
                while (IsProfit(second.bids.First().Key, main.asks.First().Key))
                {

                    if (main.asks.First().Value > second.bids.First().Value)
                    {
                        //записать в стратегию значение цены и количество в цикле пока профит
                        if (TempSt.StrategyBuy.ContainsKey(main.asks.First().Key))
                        {
                            TempSt.StrategyBuy[main.asks.First().Key] += second.bids.First().Value;
                        }
                        else
                        {
                            TempSt.StrategyBuy.Add(main.asks.First().Key, second.bids.First().Value);
                        }
                        if (TempSt.StrategySell.ContainsKey(second.bids.First().Key))
                        {
                            TempSt.StrategySell[second.bids.First().Key] += second.bids.First().Value;
                        }
                        else
                        {
                            TempSt.StrategySell.Add(second.bids.First().Key, second.bids.First().Value);
                        }

                        //отнять в списках большее значение
                        main.asks[main.asks.First().Key] -= second.bids.First().Value;
                        //удалить из списков меньшее значение
                        second.bids.Remove(second.bids.First().Key);

                    }
                    else if (main.asks.First().Value < second.bids.First().Value)
                    {
                        //записать в стратегию значение цены и количество
                        if (TempSt.StrategyBuy.ContainsKey(main.asks.First().Key))
                        {
                            TempSt.StrategyBuy[main.asks.First().Key] += main.asks.First().Value;
                        }
                        else
                        {
                            TempSt.StrategyBuy.Add(main.asks.First().Key, main.asks.First().Value);
                        }
                        if (TempSt.StrategySell.ContainsKey(second.bids.First().Key))
                        {
                            TempSt.StrategySell[second.bids.First().Key] += main.asks.First().Value;
                        }
                        else
                        {
                            TempSt.StrategySell.Add(second.bids.First().Key, main.asks.First().Value);
                        }
                        //отнять в списках большее значение
                        second.bids[second.bids.First().Key] -= main.asks.First().Value;
                        //удалить из списков меньшее значение
                        main.asks.Remove(main.asks.First().Key);
                    }

                    if (second.asks.Count == 0)
                    {
                        break;
                    }
                    if (second.bids.Count == 0)
                    {
                        break;
                    }
                    if (main.asks.Count == 0)
                    {
                        break;
                    }
                    if (main.bids.Count == 0)
                    {
                        break;
                    }

                    //равные значения будут нужны для возврата валюты 
                    //else if (temp.bids.First()[1] == i.Value.asks.First()[1])
                    //{
                    //    //записать в стратегию значение цены и количество
                    //    MainSt.StrategyBuy.Data[temp.bids.First()[0]] += i.Value.asks.First()[1];
                    //    MainSt.StrategySell.Data[i.Value.asks.First()[0]] += i.Value.asks.First()[1];

                    //    //удалить из списков меньшее значение
                    //    temp.bids.First().Clear();
                    //    i.Value.asks.First().Clear();
                    //}

                }
                return TempSt;
            }
            return new MainStrategy();
            #endregion
        }
        private static bool IsProfit(decimal Bids, decimal Asks)
        {
            if (Bids==0 || Asks==0)
            {
                return false;
            }
            if (Bids - Asks > 0)
            {
                return true;
            }
            return false;
        }
        private static bool EqualsByNull(BaseOrderModel CoinUSD)
        {
            if (CoinUSD.asks == null || CoinUSD.asks.Count() == 0 ||
                CoinUSD.bids == null || CoinUSD.bids.Count() == 0
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class ArbitrationCoinCompare
    {
        public static List<ArbitrationViewModel> ArbitrationCompare(ArbitrationModel model, decimal FirstBaseCoinBallanse = 1)
        {
            var list = new List<ArbitrationViewModel>();
            ArbitrationViewModel temp;
            temp = BTCToUSD(model, FirstBaseCoinBallanse);
            if (temp != null)
            {
                list.Add(temp);
            }
            temp = USDToBTC(model, FirstBaseCoinBallanse);
            if (temp != null)
            {
                list.Add(temp);
            }
            return list;
        }
        private static ArbitrationViewModel BTCToUSD(ArbitrationModel model, decimal FirstBaseCoinBallanse)
        {
            if (model.MarketCoinSecondBaseCoin.Bid == 0 || model.MarketCoinSecondBaseCoin.Ask == 0 ||
                   model.MarketCoinFirstBaseCoin.Bid == 0 || model.MarketCoinFirstBaseCoin.Ask == 0 ||
                   model.BaseCoinSecondBaseCoin.Bid == 0 || model.BaseCoinSecondBaseCoin.Ask == 0
                   )
            {
                return null;
            }
            //bid лево прадают ask покупают право
            //находим количество коинов которое можно купить за FirstBaseCoinBallanse 
            var QuantityOfCoins = FirstBaseCoinBallanse / model.MarketCoinFirstBaseCoin.Ask;
            //продаем коины за вторую основную валюту
            var ValueOfCoinBySecondBaseCoin = model.MarketCoinSecondBaseCoin.Bid * QuantityOfCoins;
            decimal ValueOfBallanceBySecondBaseCoin;
            if (model.Stock.TraidPairs.ExeptionPairs.ContainsKey(model.BaseCoinSecondBaseCoin_MarketName))
            {
                ValueOfBallanceBySecondBaseCoin = model.BaseCoinSecondBaseCoin.Bid / ValueOfCoinBySecondBaseCoin;
            }
            else
            {
                //продаем тоже количество FirstBaseCoinBallanse  за вторую основную валюту
                ValueOfBallanceBySecondBaseCoin = model.BaseCoinSecondBaseCoin.Ask * ValueOfCoinBySecondBaseCoin;
            }
            ValueOfBallanceBySecondBaseCoin -= (ValueOfBallanceBySecondBaseCoin * 0.0075m);
            //сравниваем если цена проданых BTC и проданых коинов
            if (FirstBaseCoinBallanse < ValueOfBallanceBySecondBaseCoin)
            {
                return new ArbitrationViewModel()
                {
                    FirstMmarket = model.MarketCoinFirstBaseCoin_MarketName,
                    SecondMarket = model.MarketCoinSecondBaseCoin_MarketName,
                    ThirdMarket = model.BaseCoinSecondBaseCoin_MarketName,
                    Stock = model.Stock.StockName,
                    CoinBuyQuantity = QuantityOfCoins,
                    CoinSellPrice = ValueOfCoinBySecondBaseCoin,
                    BTCSellPrice = ValueOfBallanceBySecondBaseCoin,
                    Profit = ValueOfBallanceBySecondBaseCoin - FirstBaseCoinBallanse
                }; 
            }
            return null;
        }
        private static ArbitrationViewModel USDToBTC(ArbitrationModel model, decimal SecondBaseCoinBallance)
        {
            if (model.MarketCoinSecondBaseCoin.Bid == 0 || model.MarketCoinSecondBaseCoin.Ask == 0 ||
                model.MarketCoinFirstBaseCoin.Bid == 0 || model.MarketCoinFirstBaseCoin.Ask == 0 ||
                model.BaseCoinSecondBaseCoin.Bid == 0 || model.BaseCoinSecondBaseCoin.Ask == 0
                )
            {
                return null;
            }
            //bid лево прадают ask покупают право
            //находим количество коинов которое можно купить за FirstBaseCoinBallanse
            var QuantityOfCoins = SecondBaseCoinBallance / model.MarketCoinSecondBaseCoin.Ask;
            //продаем коины за вторую основную валюту
            var ValueOfCoinBySecondBaseCoin = model.MarketCoinFirstBaseCoin.Bid * QuantityOfCoins;
            //продаем тоже количество SecondBaseCoinBallance  за вторую основную валюту            
            decimal ValueOfBallanceBySecondBaseCoin;
            if (model.Stock.TraidPairs.ExeptionPairs.ContainsKey(model.BaseCoinSecondBaseCoin_MarketName))
            {
                //продаем тоже количество FirstBaseCoinBallanse  за вторую основную валюту
                ValueOfBallanceBySecondBaseCoin = model.BaseCoinSecondBaseCoin.Bid * ValueOfCoinBySecondBaseCoin;
            }
            else
            {
                ValueOfBallanceBySecondBaseCoin = model.BaseCoinSecondBaseCoin.Ask / ValueOfCoinBySecondBaseCoin;

            }
            ValueOfBallanceBySecondBaseCoin -= (ValueOfBallanceBySecondBaseCoin * 0.0075m);
            //сравниваем если цена проданых BTC и проданых коинов
            if (SecondBaseCoinBallance < ValueOfBallanceBySecondBaseCoin)
            {
                return new ArbitrationViewModel()
                {
                    FirstMmarket = model.MarketCoinSecondBaseCoin_MarketName,
                    SecondMarket = model.MarketCoinFirstBaseCoin_MarketName,
                    ThirdMarket = model.BaseCoinSecondBaseCoin_MarketName,
                    Stock = model.Stock.StockName,
                    CoinBuyQuantity = QuantityOfCoins,
                    CoinSellPrice = ValueOfCoinBySecondBaseCoin,
                    BTCSellPrice = ValueOfBallanceBySecondBaseCoin,
                    Profit = ValueOfBallanceBySecondBaseCoin - SecondBaseCoinBallance
                };
            }
            return null;
        }
    }
    public class CheakEqualsCurrencies
    {
        private List<Stock> Stocks { get; set; }
        private List<Task<BaseInfoModel>> Infos { get; set; }
        private List<Task<BaseTraidPairModel>> Pairs { get; set; }
        private bool UseWalletCheak { get; set; }
        private bool UseNameCheak { get; set; }
        public CheakEqualsCurrencies(List<Stock> stocks, bool cheakNames, bool cheakWallets)
        {
            Stocks = stocks;
            Infos = GetInfos();
            Pairs = GetPairs();
            UseWalletCheak = cheakWallets;
            UseNameCheak = cheakNames;
        }
        public List<EqualsCoinModel> GetSamePairs()
        {
            var list = new List<EqualsCoinModel>();
            for (int i = 0; i < Stocks.Count(); i++)
            {
                for (int j = i + 1; j < Stocks.Count(); j++)
                {
                    var tempList = GetEquals(Pairs[i].Result.Pairs, Pairs[j].Result.Pairs, Infos[i].Result, Infos[j].Result, Stocks[i], Stocks[j]);
                    list.AddRange(tempList);
                }
            }
            return list;
        }
        private List<EqualsCoinModel> GetEquals(Dictionary<string, BaseTraidPairField> firstStockPairs,
                                    Dictionary<string, BaseTraidPairField> secondStockPairs,
                                    BaseInfoModel firstStockInfo,
                                    BaseInfoModel secondStockInfo,
                                    Stock firstStock,
                                    Stock secondStock)
        {
            var Result = new List<EqualsCoinModel>();
            foreach (var item in firstStockPairs)
            {
                if (secondStockPairs.ContainsKey(item.Key))
                {
                    var MarketCurrency = item.Value.MarketCurrency;
                    var BaseCurrency = item.Value.BaseCurrency;
                    if (CheakInfo(firstStockInfo, secondStockInfo, MarketCurrency) &&
                        CheakInfo(firstStockInfo, secondStockInfo, BaseCurrency))
                    {
                        Result.Add(new EqualsCoinModel()
                        {
                            Market = item.Key,
                            MarketCoin = firstStockInfo.CoinsInfo[MarketCurrency].Name,
                            BaseCoin = firstStockInfo.CoinsInfo[BaseCurrency].Name,
                            secMarketCoin = secondStockInfo.CoinsInfo[MarketCurrency].Name,
                            secBaseCoin = secondStockInfo.CoinsInfo[BaseCurrency].Name,
                            FirstStock = firstStock,
                            SecondStock = secondStock,
                            FirstStockAsk = item.Value.Ask,
                            SecondStockAsk = secondStockPairs[item.Key].Ask,
                            FirstStockBid = item.Value.Bid,
                            SecondStockBid = secondStockPairs[item.Key].Bid
                        });
                    }
                }
            }
            return Result;
        }
        private bool CheakInfo(BaseInfoModel firstStockInfo, BaseInfoModel secondStockInfo, string currency)
        {
            var firsSTinfo = firstStockInfo.CoinsInfo.FirstOrDefault(x => x.Key == currency);
            var secondSTinfo = secondStockInfo.CoinsInfo.FirstOrDefault(x => x.Key == currency);
            if (firsSTinfo.Key != null && secondSTinfo.Key != null)
            {
                bool IsNameTrue;
                bool IsWalletTrue;
                IsNameTrue = firsSTinfo.Value.WalletStatus && secondSTinfo.Value.WalletStatus;
                IsWalletTrue = firsSTinfo.Value.Name == secondSTinfo.Value.Name;
                if (UseWalletCheak || UseNameCheak)
                {
                    return IsNameTrue && IsWalletTrue;
                }
                if (UseNameCheak)
                {
                    return IsNameTrue;
                }
                if (UseWalletCheak)
                {
                    return IsWalletTrue;
                }
            }
            return true;
        }
        private List<Task<BaseTraidPairModel>> GetPairs()
        {
            var dict = new List<Task<BaseTraidPairModel>>();
            foreach (var item in Stocks)
            {
                dict.Add(item.TraidPairs.GetTraidPairsAsync());
            }
            return dict;
        }
        private List<Task<BaseInfoModel>> GetInfos()
        {
            var dict = new List<Task<BaseInfoModel>>();
            foreach (var item in Stocks)
            {
                dict.Add(item.Info.GetInfoAsync());
            }
            return dict;
        }
    }

}
