using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.InfoModels
{
    public class NovaInfoField
    {
        public int wallet_status { get; set; }
        public string currency { get; set; }
        public decimal wd_fee { get; set; }
        public string currencyname { get; set; }
        public int wallet_deposit { get; set; }
        public int wallet_withdrawal { get; set; }
    }
    public class NovaInfoModel:BaseInfoModel
    {
        public bool success;
        public string message;
        public List<NovaInfoField> coininfo;
        public override BaseInfoModel ToBaseInfoModel()
        {
            var baseModel = new BaseInfoModel();
            baseModel.CoinsInfo = coininfo.ToDictionary(x => x.currency.ToUpper(),
                                                    x => new BaseInfoField()
                                                    {
                                                        Name = x.currencyname.ToUpper(),
                                                        Symbol = x.currency.ToUpper(),
                                                        Status = x.wallet_status == 0,
                                                        WithdrawFee = x.wd_fee,
                                                        WalletStatus= (x.wallet_deposit==1 && x.wallet_withdrawal==1)
                                                    });


            return baseModel;
        }
    }
}
