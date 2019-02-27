using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.WalletModels
{
    public class LiveCoinBalanceField
    {
        public string type;
        public string currency;
        [JsonProperty("value")]
        public string price;
    }
    public class LiveCoinBalanceModel: BaseWalletModel
    {
        List<LiveCoinBalanceField> balance { get; set; }
        public override BaseWalletModel ToBaseWalletModel()
        {
            var baseModel = new BaseWalletModel() { };
            //foreach (var item in resalt)
            //{
            //    if (temp.ContainsKey(item.currency))
            //    {
            //        if (item.type == "available")
            //        {
            //            temp[item.currency].Available = Convert.ToDecimal(item.price);
            //        }
            //        if (item.type == "trade")
            //        {
            //            temp[item.currency].OnOrders = Convert.ToDecimal(item.price);
            //        }
            //    }
            //    else
            //    {
            //        if (item.type == "available")
            //        {
            //            var a = new TransformBallans();
            //            a.Available = Convert.ToDecimal(item.price);
            //            temp.Add(item.currency, a);
            //        }
            //        if (item.type == "trade")
            //        {
            //            var a = new TransformBallans();
            //            a.OnOrders = Convert.ToDecimal(item.price);
            //            temp.Add(item.currency, a);
            //        }
            //    }
            //}
            return baseModel;
        }
    }
}
