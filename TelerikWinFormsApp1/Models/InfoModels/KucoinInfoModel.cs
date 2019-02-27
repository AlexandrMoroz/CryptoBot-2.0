using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.InfoModels
{ public class KucoinInfoField {
        public string coinType { get; set; }
        public decimal withdrawMinFee { get; set; }
        public decimal withdrawMinAmount { get; set; }
        public string userAddressName { get; set; }
        public decimal withdrawFeeRate { get; set; }
        public bool enable { get; set; }
        public string name { get; set; }
        public bool enableWithdraw { get; set; }
        public bool enableDeposit { get; set; }
        public string coin { get; set; }
        }
    public class KucoinInfoModel:BaseInfoModel
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public List<KucoinInfoField> data { get; set; }
        public override BaseInfoModel ToBaseInfoModel()
        {
            var baseModel = new BaseInfoModel();
            baseModel.CoinsInfo = data.ToDictionary(x => x.coin.ToUpper(),
                                                    x => new BaseInfoField()
                                                    {
                                                        Name = x.name.ToUpper(),
                                                        Symbol = x.coin.ToUpper(),
                                                        Status = x.enable,
                                                        WithdrawFee = x.withdrawMinFee,
                                                        WalletStatus = x.enableWithdraw&&x.enableDeposit,
                                                        MinWithdrawAmount=x.withdrawMinAmount
                                                    });



            return baseModel;
        }

    }
}
