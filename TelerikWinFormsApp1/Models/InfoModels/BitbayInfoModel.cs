using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Models.InfoModels
{public class BitbayInfoField
    {
        public string available { get; set; }
        public string locked { get; set; }
    }
    public class BitbayInfoModel:BaseInfoModel
    {
        public Dictionary<string,BitbayInfoField>  balances { get; set; }
        public Dictionary<string, string> addresses { get; set; }
        private Dictionary<string, string> ExceptionName { get; set; } = new Dictionary<string, string>();
        public BitbayInfoModel()
        {

        }
        public override BaseInfoModel ToBaseInfoModel()
        {
            var baseModel = new BaseInfoModel();
            var symbolName = GetSymbolAndName();
            baseModel.CoinsInfo = balances.ToDictionary(x => x.Key.ToUpper(),
                                                    x => new BaseInfoField()
                                                    {
                                                        Name = CheakName(x.Key.ToUpper(), symbolName.First(y => y.Key == x.Key).Value.ToUpper()),
                                                        Symbol = x.Key.ToUpper(),
                                                        Status = true,
                                                        WalletStatus = true
                                                    });


            return baseModel;
        }
        private string CheakName(string coinSymbol, string coinName)
        {
            if (ExceptionName.ContainsKey(coinSymbol))
            {
                return ExceptionName[coinSymbol];
            }
            return coinName;
        }
        private Dictionary<string, string> GetSymbolAndName()
        {

            var data = new Dictionary<string, string>();
            var url = "https://bitbay.net/en";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var table = doc.DocumentNode.SelectNodes("//div[@class='flex-currency']/div[@class='currency-item']/a/p");
            for (int i = 0; i < table.Count; i++)
            {
                var symbol = table[i].ChildNodes[0].InnerText;
                var name = table[i].ChildNodes[1].InnerText;
                data.Add(symbol, name);
            }

            return data;
        }
    }
    
}
