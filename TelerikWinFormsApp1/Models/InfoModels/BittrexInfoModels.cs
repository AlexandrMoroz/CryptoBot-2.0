using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Stocks;

namespace Cryptobot.Models.InfoModels
{
    public class BittrexWalletStatusHealth
    {
        public int MinutesSinceBHUpdated { get; set; }
        public bool isActive { get; set; }
        public string Currency { get; set; }
    }
    public class BittrexWalletStatusCurrency
    {
        public string CurrencyLong { get; set; }
        public string Currency { get; set; }
        public decimal TxFee { get; set; }
    }
    public class BittrexMainField
    {
        public BittrexWalletStatusHealth Health { get; set; }
        public BittrexWalletStatusCurrency Currency { get; set; }
    }
    public class BittrexWalletStatusModel
    {
        public List<BittrexMainField> result { get; set; }
        public Dictionary<string,bool> GetSymbolWalletStatus()
        {
            var dict = new Dictionary<string, bool>();
            foreach (var item in result)
            {
                var status = item.Health.isActive && item.Health.MinutesSinceBHUpdated < 30 ? true : false;
                dict.Add(item.Currency.Currency, status);
            }
            return dict;
        }
    }
    public class BittrexInfoField
    {
        public string Currency;
        public decimal TxFee;
        public bool IsActive;
        public string CurrencyLong;
        public short MinConfirmation;
        public string CoinType;
        public string BaseAddress;
    }
    public class BittrexInfoModel : BaseInfoModel
    {
        public bool success;
        public string message;
        public List<BittrexInfoField> result;

        public BittrexInfoModel()
        {

        }
        public override BaseInfoModel ToBaseInfoModel()
        {
            var baseModel = new BaseInfoModel();
            var walletStatus = new BittrexWalletStatus().GetWalletStatus().GetSymbolWalletStatus();
            baseModel.CoinsInfo = result.ToDictionary(x => x.Currency.ToUpper(),
                                                    x => new BaseInfoField()
                                                    {
                                                        Name = CheakName(x.Currency.ToUpper(),x.CurrencyLong.ToUpper()),
                                                        Symbol = x.Currency.ToUpper(),
                                                        Status = x.IsActive,
                                                        WithdrawFee = x.TxFee,
                                                        WalletStatus = walletStatus.First(y => y.Key == x.Currency.ToUpper()).Value
                                                    });
            return baseModel;
        }

    }
}
