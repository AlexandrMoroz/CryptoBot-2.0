using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.InfoModels
{
    public class YobitInfoField
    {
        public decimal fee { get; set; }
        public decimal fee_buyer { get; set; }
        public decimal fee_seller { get; set; }
        public decimal min_amount { get; set; }
    }
    public class YobitInfoModel: BaseInfoModel
    {
        public Dictionary<string, YobitInfoField> pairs { get; set; }
        public override BaseInfoModel ToBaseInfoModel()
        {
            var symbolName = GetSymbolAndName();
            var BaseInfo = new BaseInfoModel();
            foreach (var item in pairs)
            {
                var symbol = item.Key.Split('_')[0].ToUpper();
                if (!BaseInfo.CoinsInfo.ContainsKey(symbol))
                {
                    BaseInfo.CoinsInfo.Add(symbol, new BaseInfoField() {
                        Symbol = symbol,
                        Name = symbolName.First(x => x.Key == symbol).Value.ToUpper(),
                        MinOrderAmount = item.Value.min_amount
                    });
                }
            }
            return BaseInfo;
        }
        private Dictionary<string, string> GetSymbolAndName()
        {
            var data = new Dictionary<string, string>();
            var url = "https://yobit.net/ru/coinsinfo";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var table = doc.DocumentNode.SelectNodes("//table[@id='downloads_table']/tbody/tr");
            foreach (var item in table)
            {
                var symbol = item.ChildNodes[1].InnerText;
                var name = item.ChildNodes[3].InnerText;
                data.Add(symbol, name);
            }
            return data;
        }
    }
}
