using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cryptobot.AIClass;
using Cryptobot.Models.InfoModels;
using Cryptobot.Models.OrdersModels;
using Cryptobot.Models.TraidPairsModels;

namespace Cryptobot.Stocks
{
    public interface IGetTraidPairs
    {
        Dictionary<string, string> ExeptionPairs { get; set; }
        Task<BaseTraidPairModel> GetTraidPairsAsync();

    }
    public interface IGetInfo
    {

        Task<BaseInfoModel> GetInfoAsync();

    }
    public interface IGetOrders
    {

        Task<BaseOrdersModel> GetAllOrdersAsync();

        Task<BaseOrdersModel> GetOrdersAsync(List<KeyValuePair<string, string>> arg);
        /// <summary>
        /// MainCoin can be altcoin you want, second BTC USD ETH LTC USDT or else main coin of stock
        /// </summary>
        /// <returns></returns>
        Task<BaseOrderModel> GetOrderAsync(string MainCoinName, string SecondCoinName);
    }

    public interface IWallet
    {
        Dictionary<string, TransformBallans> GetBalances();
        Dictionary<string, string> GetDepositAddresses();
        Task<Dictionary<string, TransformBallans>> GetBalancesAsync();

        /// <summary>Returns all of your deposit addresses.</summary>
        Task<Dictionary<string, string>> GetDepositAddressesAsync();


    }
    public interface ITrading
    {
        Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote);

        //Task<IList<IOrder>> GetOpenOrdersAsync(CurrencyPair currencyPair);
        Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote);
    }



    public class Stock : ICheakStockApi
    {
        public bool HasWalletStatus { get; set; }
        public bool HasCoinsName { get; set; }
        public string StockName { get; set; }
        public IGetInfo Info { get; set; }
        public IGetOrders Orders { get; set; }
        public IWallet Ballans { get; set; }
        public ITrading Traid { get; set; }
        public IGetTraidPairs TraidPairs { get; set; }
        public Stock() { }
        public Stock(Stock arg)
        {
            StockName = arg.StockName;
            Info = arg.Info;
            Orders = arg.Orders;
            Ballans = arg.Ballans;
            Traid = arg.Traid;
            TraidPairs = arg.TraidPairs;
        }
        public Stock(string stockname, IGetInfo info, IGetOrders orders, IWallet ballans, ITrading traid, IGetTraidPairs traidpairs)
        {
            StockName = StockName;
            Info = info;
            Orders = orders;
            Ballans = ballans;
            Traid = traid;
            TraidPairs = traidpairs;
        }

        public bool CheakStockInfo()
        {
            try
            {
                var info = Info.GetInfoAsync().Result;
                return true;
            }
            catch (Exception)
            {
                
                return false;
            }

        }

        public bool CheakStockPairs()
        {
            try
            {
                var pairs = TraidPairs.GetTraidPairsAsync().Result;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheakStockOrders()
        {
            try
            {
                var order = Orders.GetOrderAsync("ETH","BTC").Result;
                return true;
            }
            catch (Exception)
            {
                
                return false;
            }
        }
        public bool CheakStock()
        {
            if (this.CheakStockInfo() &&  this.CheakStockPairs())//this.CheakStockOrders() &&
            {
                return true;
            }
            MessageBox.Show(StockName + " not available");
            return false;
        }
    }
}
