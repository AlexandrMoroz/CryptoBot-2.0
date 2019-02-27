using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.InfoModels
{
    public class PoloniexInfoField
    {
        public int id { get; set; }
        public string name { get; set; }
        public string humanType { get; set; }
        public string txFee { get; set; }
        public string minConf { get; set; }
        public string depositAddress { get; set; }
        public int disabled { get; set; }
        public int delisted { get; set; }
        public int frozen { get; set; }
        public bool isGeofenced { get; set; }
    }
    public class PoloniexInfoModel: BaseInfoModel
    {

        public Dictionary<string, PoloniexInfoField> pairs { get; set; }
        private Dictionary<string, string> StockBaseCoins { get; set; }
        public PoloniexInfoModel()
        {
            StockBaseCoins = new Dictionary<string, string>();

        }
        public override BaseInfoModel ToBaseInfoModel()
        {
            var baseInfoModel = new BaseInfoModel();
            baseInfoModel.CoinsInfo = pairs.ToDictionary(x => x.Key.ToUpper(), y => new BaseInfoField()
            {
                Name = CheakName(y.Key.ToUpper(),y.Value.name.ToUpper()),
                Status = CheakWallet(y.Value),
                WithdrawFee = ToDecimal(y.Value.txFee),
                Symbol = y.Key.ToUpper(),
                WalletStatus= CheakWallet(y.Value)

            });
            return baseInfoModel;
            
        }

        private decimal ToDecimal(string str)
        {
            return Decimal.Parse(str, System.Globalization.NumberStyles.AllowDecimalPoint | System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
        private bool CheakWallet(PoloniexInfoField arg)
        {
            List<bool> temp = new List<bool>();
            temp.Add(arg.disabled == 1 ? false : true);
            temp.Add(arg.delisted == 1 ? false : true);
            temp.Add(arg.frozen == 1 ? false : true);

            return !temp.Any(x => x == false);

        }
    }
}
