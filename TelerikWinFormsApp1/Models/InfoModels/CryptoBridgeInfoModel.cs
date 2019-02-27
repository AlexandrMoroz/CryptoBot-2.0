using System;
using System.Collections.Generic;
using System.Linq;

namespace Cryptobot.Models.InfoModels
{
    public class CryptoBridgeInfoField
    {
        public string coinType { get; set; }
        public string walletName { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public string transactionFee { get; set; }
        public bool restricted { get; set; }
        public bool depositAllowed { get; set; }
        public bool withdrawalAllowed { get; set; }
    }
    public class CryptoBridgeInfoModel : BaseInfoModel
    {
        private Dictionary<string, string> ExceptionNames { get; set; } = new Dictionary<string, string>();

        public CryptoBridgeInfoModel(List<CryptoBridgeInfoField> arg)
        {
            info = arg;
        }
        public List<CryptoBridgeInfoField> info { get; set; }
        public override BaseInfoModel ToBaseInfoModel()
        {
            var baseModel = new BaseInfoModel();
            baseModel.CoinsInfo = info.ToDictionary(x => CheakSymbol(x.coinType.Split('.')[1].ToUpper()),
                                                    x => new BaseInfoField()
                                                    {
                                                        Name = CheakName(x.coinType.Split('.')[1].ToUpper(),x.name.ToUpper()),
                                                        Symbol = CheakSymbol(x.coinType.Split('.')[1].ToUpper()),
                                                        Status = !x.restricted,
                                                        WithdrawFee = ToDecimal(x.transactionFee),
                                                        WalletStatus = (x.depositAllowed && x.withdrawalAllowed)
                                                    });


            return baseModel;
        }

        private decimal ToDecimal(string str)
        {
            return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
    }
}
