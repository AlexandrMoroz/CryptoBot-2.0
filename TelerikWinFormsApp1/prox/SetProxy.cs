
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot
{
    class SetProxy
    {
        String ProxyAdress;
        int Port;
        public SetProxy(string proxyAdress, int port)
        {
            ProxyAdress = proxyAdress;
            Port = port;
        }
        public WebResponse GetResponse(string site)
        {
            List<string> ProxyList = GetProxys();
            WebResponse resp = null;
            foreach (var item in ProxyList)
            {

                WebProxy proxy = new WebProxy(item.Split(':')[0], Convert.ToInt32(item.Split(':')[1]));
                var req = (HttpWebRequest)HttpWebRequest.Create(site);
                req.Proxy = proxy;

                try
                {
                    resp = req.GetResponse();
                    if (resp == null)
                    {
                        continue;
                    }
                    else
                    {
                        return resp;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            return resp;
           
        }
        private List<string> GetProxys()
        {
                        List <string> tp = new List<string>();
            using (StreamReader stream = new StreamReader(
                   "prox/proxy.txt"))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    tp.Add(line);
                }
            }
            return tp;
        }
    }
}
