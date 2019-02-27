using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot
{
    public static class GetRequst
    {
 
        public static WebResponse ProxyRequst(string site, string ip, int port)
        {
            //"162.243.140.150" 80
            SetProxy prox = new SetProxy(ip, port);
            return  prox.GetResponse(site);
        }
        public static WebResponse Requst(string site)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            return resp;
        }
    }

    public static class PoloniexGetRequst
    {
        public static WebResponse ProxyRequst(string arg)
        {
            SetProxy prox = new SetProxy("162.243.140.150", 80);
            WebResponse resp = prox.GetResponse(arg);
            return resp;
        }
        public static WebResponse Requst(string arg)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create(arg);
            WebResponse resp = req.GetResponse();
            return resp;
        }
    }

    public static class LiveCoinGetRequst
    {

        private static string HashHMAC(string key, string message)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return ByteArrayToString(hashmessage);
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public static WebResponse Requst(string site)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp=null;
            try
            {
                resp = req.GetResponse();
            }
            catch(WebException ex)
            {
                
            }
            return resp;

        }
        public static WebResponse AuthRequst(string relativUrl, string data)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(relativUrl);
            WebResponse resp = null;
            request.Headers["Api-Key"] = LiveCoinKey.ApiKey;
            request.Headers["Sign"] = HashHMAC(LiveCoinKey.SecretKey, data).ToUpper();
            try
            {
                resp = request.GetResponse();
            }
            catch (WebException ex)
            {

            }
            return resp;
        }
    }

    public static class CryptoGetRequst
    {
        public static WebResponse Requst(string arg)
        {
            return GetRequst.Requst(arg);
        }
    }
    public static class BittrexGetRequst {
        public static WebResponse Requst(string arg)
        {
            return GetRequst.Requst(arg);
        }
    }
    public static class ExmoGetRequst
    {
        public static WebResponse Requst(string arg)
        {
            return GetRequst.Requst(arg);
        }
    }
}
