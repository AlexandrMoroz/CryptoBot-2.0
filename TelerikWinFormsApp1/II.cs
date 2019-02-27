using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Interfesse;
using Cryptobot.AIClass;
namespace Cryptobot
{
    /// <summary>
    /// +обнавление цен и проверка их на профит
    /// +проверка активны ли адресса
    /// +запрос балланса по указаной бирже если не активна то  
    /// +запрос цены битка
    /// +расчет профита баланс + цена битка
    /// 
    /// +вывод покупка количество ценна общая сумма
    ///       продажа количество ценна общая сумма
    ///       профит
    /// 
    /// +отправка ордера биржа покупки 
    /// +проверка баланса по валюте ордера 
    /// 
    /// +отправка перевода 
    /// 
    /// +проверка баланса биржи продажи
    /// +отправка ордера на продажу
    /// +проверка баланса по продажи ордера
    /// </summary>
    //public class II
    //{
    //    public MainStrategy mn;
    //    public ProfitCalc profit;
        
    //    public II(MainStrategy arg)
    //    {
    //        mn = arg;
    //        profit = new ProfitCalc(mn);
    //    }
    //    public bool UpdatePrice()
    //    {
    //        var SellSTPrice = mn.SellStockEX.Orders.GetOrderAsync(mn.MarketName.Split(AccseptCoins.SPLITER)[0], mn.MarketName.Split(AccseptCoins.SPLITER)[1]);
    //        var BuySTPrice = mn.BuyStockEX.Orders.GetOrderAsync(mn.MarketName.Split(AccseptCoins.SPLITER)[0], mn.MarketName.Split(AccseptCoins.SPLITER)[1]);

    //        MainStrategy tempStr = CompairCoins.CoinCompare(SellSTPrice.Result, BuySTPrice.Result, mn.BuyStockEX, mn.BuyStockEX, mn.MarketName);
    //        if (tempStr.StrategyBuy.Count != 0 && tempStr.StrategySell.Count != 0)
    //        {
    //            mn.StrategyBuy = tempStr.StrategyBuy;
    //            mn.StrategySell = tempStr.StrategySell;
    //            return true;
    //        }
    //        return false;

    //    }
    //    public decimal GetProfit(decimal quantity = 0)
    //    {
    //        if (quantity == 0)
    //        {
    //            return profit.Profit;
    //        }
    //        else
    //        {
    //            var prof = new ProfitCalc(mn, quantity);
    //            return prof.Profit;
    //        }
    //    }
    //    public bool CheakIsActive(string currenty)
    //    {
    //        var tbuy = mn.BuyStockEX.Info.GetInfoAsync();
    //        var tsell = mn.BuyStockEX.Info.GetInfoAsync();
    //        var buy = tbuy.Result;
    //        if (buy[currenty].Status == false)
    //        {
    //            return false;
    //        }
    //        var sell = tsell.Result;
    //        if (sell[currenty].Status == false)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }
    //    /// <summary>  return Dict of 2 elem 0 = "BUY", 1 = "SELL";</summary>                        
    //    public Dictionary<string, CoinBuySellView> BuySellView()
    //    {
           
           
    //        var temp = new Dictionary<string, CoinBuySellView>()
    //        {
    //            {   "BUY",
    //                new CoinBuySellView(){
    //                Name = mn.BuyStockEX.StockName,
    //                PriceQantity = profit.StrategyBuy.DictionaryViewString()}
    //            },
    //            {   "SELL",
    //                new CoinBuySellView(){
    //                Name = mn.SellStockEX.StockName,
    //                PriceQantity = profit.StrategySell.DictionaryViewString()
    //                }
    //            }
    //        };
    //        return temp;
    //    }
    //    public bool BuyOrder()
    //    {
    //        foreach (var item in profit.StrategyBuy)
    //        {
    //            var resp = mn.BuyStockEX.Traid.PostOrderAsync(mn.MarketName, OrderType.Buy, item.Key, item.Value);
    //            if (resp.Result == "")
    //            {
    //                throw new Exception("Cant buy in" + mn.BuyStockEX.StockName);
    //            }
    //        }
    //        return true;

    //    }
    //    public bool SellOrder()
    //    {
    //        foreach (var item in profit.StrategySell)
    //        {
    //            var resp = mn.SellStockEX.Traid.PostOrderAsync(mn.MarketName, OrderType.Sell, item.Key, item.Value);
    //            if (resp.Result == "")
    //            {
    //                throw new Exception("Cant buy in" + mn.BuyStockEX.StockName);
    //            }
    //        }
    //        return true;
    //    }
    //    public bool IsOrderComplite(OrderType arg)
    //    {
    //        var bal = mn.BuyStockEX.Ballans.GetBalancesAsync();
    //        if (arg == OrderType.Buy)
    //        {
    //            var temp = bal.Result.Where(x => x.Key == mn.MarketName).FirstOrDefault();
    //            if(temp.Value.Available != 0m)
    //            {
    //                return true;
    //            }
    //            else if (temp.Value.OnOrders != 0m)
    //            {
    //                return false;
    //            }
    //        }
    //        else if (arg == OrderType.Sell)
    //        {
    //            var temp = bal.Result.Where(x => x.Key == mn.MarketName).FirstOrDefault();
    //            if (temp.Value.Available != 0m)
    //            {
    //                return true;
    //            }
    //            else if (temp.Value.OnOrders != 0m)
    //            {
    //                return false;
    //            }
    //        }
    //        return false;
    //    }
    //    public TransformWithdrow DoWithdrawal()
    //    {
    //        var DestinationAdress = mn.SellStockEX.Ballans.GetDepositAddressesAsync().Result
    //                                .Where(x => x.Key.ToUpper() == mn.MarketName.ToUpper()).FirstOrDefault().Value;
    //        if (DestinationAdress != null) {
    //            var quantity = profit.GetBuyStrategy(false).Sum(x => x.Value);
    //            var result =  mn.BuyStockEX.Traid.PostWihdrowAsync(mn.MarketName, DestinationAdress, quantity).Result;
    //            return result;  
    //        }
    //        else
    //        {
    //            throw new Exception("Can't do withdrowal");
    //        }
    //    }

    //    public bool IsWithdrowalComplite()
    //    {
    //        var ballans = mn.SellStockEX.Ballans.GetBalancesAsync().Result;
    //        if (ballans[mn.MarketName].Available != 0)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}
}
