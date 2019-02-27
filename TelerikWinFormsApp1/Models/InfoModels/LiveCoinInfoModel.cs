using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.InfoModels
{
    public class LiveCoinInfoFields
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public string walletStatus { get; set; }
        public decimal withdrawFee { get; set; }
        public decimal? difficulty { get; set; }
        public decimal minDepositAmount { get; set; }
        public decimal minWithdrawAmount { get; set; }
        public decimal minOrderAmount { get; set; }
    }

    public class LiveCoinInfoModel : BaseInfoModel
    {
        public bool success;
        public List<LiveCoinInfoFields> info;
        public LiveCoinInfoModel()
        {

        }

        public override BaseInfoModel ToBaseInfoModel()
        {
            var baseModel = new BaseInfoModel();
            baseModel.CoinsInfo = info.ToDictionary(x => x.symbol.ToUpper(),
                                                    x => new BaseInfoField()
                                                    {
                                                        Name = CheakName(x.symbol.ToUpper(),x.name.ToUpper()),
                                                        Symbol = x.symbol.ToUpper(),
                                                        WalletStatus = CheakWallet(x.walletStatus),
                                                        WithdrawFee = x.withdrawFee,
                                                        Difficulty = IsNull(x.difficulty),
                                                        MinDepositAmount = x.minDepositAmount,
                                                        MinWithdrawAmount = x.minWithdrawAmount,
                                                        MinOrderAmount = x.minOrderAmount
                                                    });

            baseModel.CoinsInfo.Add("USD", new BaseInfoField()
            {
                Name = "USD",
                Symbol = "USD",
                Status = true,
                WalletStatus = true,
            });
            baseModel.CoinsInfo.Add("EUR", new BaseInfoField()
            {
                Name = "EUR",
                Symbol = "EUR",
                Status=true,
                WalletStatus = true,
            });
            return baseModel;
        }
        private bool CheakWallet(string arg)
        {
            return arg != "normal" ? false : true;

        }
        private decimal IsNull(decimal? arg) {
            return (decimal) (arg == null ? 0 : arg);
        }
    }
}
