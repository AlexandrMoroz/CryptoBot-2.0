using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.InfoModels
{
    public class CoinexInfoField
    {
        public int CurrencyID { get; set; }
        public string Name { get; set; }
        public string TickerCode { get; set; }
        public string WalletStatus { get; set; }
        public string Type { get; set; }
    }
    public class CoinexInfoModel : BaseInfoModel
    {
        public int success { get; set; }
        public string message { get; set; }
        public List<CoinexInfoField> result { get; set; }
        public override BaseInfoModel ToBaseInfoModel()
        {
            var baseInfo = new BaseInfoModel();
            baseInfo.CoinsInfo = result.ToDictionary(x => x.TickerCode.ToUpper(), y => new BaseInfoField()
            {
                Name = y.Name.ToUpper(),
                Symbol = y.TickerCode,
                WalletStatus = CheakWallet(y.WalletStatus),
            });
            return baseInfo;
        }
        private bool CheakWallet(string arg)
        {
            return arg != "online" ? false : true;
        }
    }
}
