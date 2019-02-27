using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; 
namespace Cryptobot
{
    public class ProfitCalc
    {
        public int BTC = 6400;
        public decimal TradeFeeBuy = 0.002m;
        public decimal TradeFeeSell = 0.002m;
        public Dictionary<decimal, decimal> StrategyBuy { get; set; }
        public Dictionary<decimal, decimal> StrategySell { get; set; }
        public decimal BuyBTC { get; set; }
        public decimal BuyFee { get; set; }
        public decimal SellBTC { get; set; }
        public decimal SellFee { get; set; }  
        public decimal BTCStockContains { get; set; }   
        public decimal Profit { get; set; }  
        public MainStrategy MStrategy { get; set; }
        public ProfitCalc(MainStrategy arg, decimal BTCValue)
        {
            MStrategy = arg;
            CalcBuyStrategy(BTCValue);
            CalcSellStrategy();
            CalcProfit();
        }
        public ProfitCalc(MainStrategy arg)
        {
            MStrategy = arg;
            CalcBuyStrategy();
            CalcSellStrategy();
            CalcProfit();
            BTCStockContains = GetBTCBallans();
        }
        private void CalcBuyStrategy(decimal BTCValue = 0)
        {
            decimal BTC;
            if (BTCValue == 0)
            {
                BTC = GetBTCBallans();
            }
            else
            {
                BTC = BTCValue;
            }

            var Buy = new Dictionary<decimal, decimal>(MStrategy.StrategyBuy);

            Dictionary<decimal, decimal> BuyTemp = new Dictionary<decimal, decimal>();
            
            //расчет количества ордеров на покупку по текущий баланс битка на 
            //биржи покупки (какие ордера надо покупать)
            while (Buy.Count != 0 && BTC != 0)
            {
                var BuyElem = Buy.First();

                if (BTC / BuyElem.Key > BuyElem.Value)
                {
                    if (BuyTemp.ContainsKey(BuyElem.Key))
                    {
                        BuyTemp[BuyElem.Key] += BuyElem.Value;
                    }
                    else
                    {
                        BuyTemp.Add(BuyElem.Key, BuyElem.Value);
                    }
                    Buy.Remove(BuyElem.Key);
                    BTC -= BuyElem.Value * BuyElem.Key;
                }
                else if (BTC / BuyElem.Key <= BuyElem.Value)
                {
                    if (BuyTemp.ContainsKey(BuyElem.Key))
                    {
                        BuyTemp[BuyElem.Key] += BTC / BuyElem.Key;
                    }
                    else
                    {
                        BuyTemp.Add(BuyElem.Key, BTC / BuyElem.Key);
                    }

                    break;
                }
            }
            //отнимаем от ордеров продажи количество валюты равное 0.2% от общего числа
            decimal BuyValue = BuyTemp.Sum(x => x.Key * x.Value);

            BuyBTC = Math.Round(BuyValue,8);
            StrategyBuy = BuyTemp;
        }
        private void CalcSellStrategy()
        {
            var Sell = MStrategy.StrategySell;
            var BuyTemp = new Dictionary<decimal, decimal>(StrategyBuy);
            Dictionary<decimal, decimal> SellResult = new Dictionary<decimal, decimal>();
            //расчет количества ордеров на продажу по ордерам буржи продажи  (по каким ордерам надо продавать) 
            while (Sell.Count != 0 && BuyTemp.Count != 0)
            {
                var SellElem = Sell.First();
                var BuyElem = BuyTemp.First();

                if (BuyElem.Value > SellElem.Value)
                {
                    if (SellResult.ContainsKey(SellElem.Key))
                    {
                        SellResult[SellElem.Key] +=  SellElem.Value;
                    }
                    else
                    {
                        SellResult.Add(SellElem.Key, SellElem.Value);
                    }
                    Sell.Remove(SellElem.Key);
                    BuyTemp[BuyElem.Key] -= SellElem.Value;
                    continue;
                }
                else if (BuyElem.Value < SellElem.Value)
                {
                    if (SellResult.ContainsKey(SellElem.Key))
                    {
                        SellResult[SellElem.Key] += BuyElem.Value;
                    }
                    else
                    {
                        SellResult.Add(SellElem.Key, BuyElem.Value);
                    }
                    BuyTemp.Remove(BuyElem.Key);
                    Sell[SellElem.Key] -= BuyElem.Value;
                    continue;
                }
                else if (BuyElem.Value == SellElem.Value)
                {
                    if (SellResult.ContainsKey(SellElem.Key))
                    {
                        SellResult[SellElem.Key] += BuyElem.Value;
                    }
                    else
                    {
                        SellResult.Add(SellElem.Key, BuyElem.Value);
                    }
                    BuyTemp.Remove(BuyElem.Key);
                    Sell.Remove(SellElem.Key);
                    continue;
                }
            }

            decimal SellValue = SellResult.Sum(x => x.Key * x.Value);
            SellValue -= SellValue * TradeFeeSell;

            SellBTC = Math.Round(SellValue,8);
            StrategySell = SellResult;
        }
        private void CalcProfit()
        {
            Profit = SellBTC - BuyBTC;
            BuyFee = Math.Round(BuyBTC * TradeFeeBuy,8);
            SellFee = Math.Round(SellBTC * TradeFeeSell,8);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fee">true for get strategy whis fee, false whis out</param>
        /// <returns></returns>
        public Dictionary<decimal, decimal> GetBuyStrategy(bool fee)
        {
            if (fee)
            {
                var Price = StrategyBuy.First().Key;
                var temp = StrategyBuy;
                temp[Price] -= Price * (BuyBTC * TradeFeeBuy);
                return temp;
            }
            else
            {
                return StrategyBuy;
            }
        }
        public decimal GetBTCBallans()
        {
            var bal = MStrategy.BuyStockEX.Ballans.GetBalances();
            if (bal.ContainsKey("BTC"))
            {
                return bal["BTC"].Available;
            }
            else
            {
                throw new Exception("No BTC value");
            }
        }
    }
}
