using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Cryptobot
{
    public static class PoloniexPostRequst
    {
        internal const string ApiUrlHttpsBase = "https://poloniex.com/";
        internal const string ApiUrlHttpsRelativeTrading = "tradingApi";
        internal static readonly DateTime DateTimeUnixEpochStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        internal static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;
        private static UInt64 CurrentHttpPostNonce { get; set; }
        private static string ToStringHex(this byte[] value)
        {
            var output = string.Empty;
            for (var i = 0; i < value.Length; i++)
            {
                output += value[i].ToString("x2", InvariantCulture);
            }
            return (output);
        }
        public static string PostString(string relativeUrl, string postData)
        {
            var request = WebRequest.CreateHttp(ApiUrlHttpsBase + relativeUrl);
            request.Method = "POST";
            request.Proxy = new WebProxy("162.243.140.150", 80);
            request.Timeout = Timeout.Infinite;
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentType = "application/x-www-form-urlencoded";

            var postBytes = Encoding.ASCII.GetBytes(postData);
            request.ContentLength = postBytes.Length;

            var key = Encoding.ASCII.GetBytes(PoloniexKey.SecretKey);
            request.Headers["Key"] = PoloniexKey.ApiKey;

            var a = new HMACSHA512(key).ComputeHash(postBytes).ToStringHex();
            request.Headers["Sign"] = a;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }
            string temp = "";
            try
            {
                WebResponse resp = request.GetResponse();

                using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
                {
                    temp = stream.ReadToEnd();
                }
                return temp;
            }
            catch (WebException e)
            {
                string message = e.Message;
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "POLONIEX", buttons);
                return temp;
            }
        }
        public static string GetCurrentHttpPostNonce()
        {
            var newHttpPostNonce = Convert.ToUInt64(Math.Round(DateTime.UtcNow.Subtract(DateTimeUnixEpochStart).TotalMilliseconds * 1000, MidpointRounding.AwayFromZero));
            if (newHttpPostNonce > CurrentHttpPostNonce)
            {
                CurrentHttpPostNonce = newHttpPostNonce;
            }
            else
            {
                CurrentHttpPostNonce += 1;
            }

            return CurrentHttpPostNonce.ToString(InvariantCulture);
        }
    }

    public static class LiveCoinPostRequst
    {
        private static string ApiUrlHttpsBase = "https://api.livecoin.net/";
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
        public static string PostString(string relativeUrl, string postData)
        {
            var request = WebRequest.CreateHttp(ApiUrlHttpsBase + relativeUrl);
            request.Method = "POST";
            //request.Proxy = new WebProxy("162.243.140.150", 80);
            request.Timeout = Timeout.Infinite;
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentType = "application/x-www-form-urlencoded";

            var postBytes = Encoding.ASCII.GetBytes(postData);
            request.ContentLength = postBytes.Length;
            request.Headers["Api-Key"] = LiveCoinKey.ApiKey;

            string Sign = HashHMAC(LiveCoinKey.SecretKey, postData).ToUpper();
            request.Headers["Sign"] = Sign;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }
            WebResponse resp = request.GetResponse();
            string temp;
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                temp = stream.ReadToEnd();
            }
            return temp;
        }

    }

    public static class CryptoriaPostRequst
    {
        internal const string ApiUrlHttpsBase = "https://www.cryptopia.co.nz/api/";
        public static string PostString(string relativeUrl, string postData)
        {

            string requestUri = ApiUrlHttpsBase + relativeUrl;

            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "POST";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentType = "application/x-www-form-urlencoded";

            // Authentication
            string requestContentBase64String = string.Empty;
            var postBytes = Encoding.ASCII.GetBytes(postData);

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }
            // Hash content to ensure message integrity
            using (var md5 = MD5.Create())
            {
                requestContentBase64String = Convert.ToBase64String(md5.ComputeHash(postBytes));
            }

            //create random nonce for each request
            var nonce = Guid.NewGuid().ToString("N");

            //Creating the raw signature string
            var signature = Encoding.UTF8.GetBytes(string.Concat(CryptopiaKey.ApiKey, HttpMethod.Post, HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower()), nonce, requestContentBase64String));
            using (var hmac = new HMACSHA256(Convert.FromBase64String(CryptopiaKey.SecretKey)))
            {
                request.Headers[HttpRequestHeader.Authorization] = "amx " + string.Format("{0}:{1}:{2}", CryptopiaKey.ApiKey, Convert.ToBase64String(hmac.ComputeHash(signature)), nonce);
            }

            // Send Request
            WebResponse resp = request.GetResponse();
            string temp;
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                temp = stream.ReadToEnd();
            }
            return temp;
        }

    }

    public static class BittrexPostRequst
    {
        const string ApiCallTemplate = "https://bittrex.com/api/{0}/{1}";
        const string ApiVersion = "v1.1";
        private static string HashHmac(string message, string secret)
        {
            Encoding encoding = Encoding.UTF8;
            using (HMACSHA512 hmac = new HMACSHA512(encoding.GetBytes(secret)))
            {
                var msg = encoding.GetBytes(message);
                var hash = hmac.ComputeHash(msg);
                return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
            }
        }
        public static string PostString(string relativeUrl, string postData)
        {
            var nonce = DateTime.Now.Ticks;
            var uri = string.Format(ApiCallTemplate, ApiVersion, relativeUrl + "?apikey=" + BittrexKey.ApiKey + "&nonce=" + nonce) + postData;
            var sign = HashHmac(uri, BittrexKey.SecretKey);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers["apisign"] = sign;


            var postBytes = Encoding.ASCII.GetBytes("");
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }

            WebResponse resp = request.GetResponse();
            string temp;
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                temp = stream.ReadToEnd();
            }

            return temp;
        }
    }
    public static class GatePostRequst
    {
        const string ApiCallTemplate = "https://bter.com/api2/1/{0}";
        private static string HashHmac(string message, string secret)
        {
            Encoding encoding = Encoding.UTF8;
            using (HMACSHA512 hmac = new HMACSHA512(encoding.GetBytes(secret)))
            {
                var msg = encoding.GetBytes(message);
                var hash = hmac.ComputeHash(msg);
                return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
            }
        }
        public static string PostString(string relativeUrl, string postData)
        {
            var sign = HashHmac(postData, BterKey.SecretKey);

            var request = (HttpWebRequest)WebRequest.Create(ApiCallTemplate + relativeUrl);
            request.Method = "POST";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers["SIGN"] = sign;
            request.Headers["KEY"] = BterKey.ApiKey;

            var postBytes = Encoding.ASCII.GetBytes(postData);
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }

            WebResponse resp = request.GetResponse();
            string temp;
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                temp = stream.ReadToEnd();
            }

            return temp;
        }
    }
    public static class YoubitPostRequst
    {
        private static readonly DateTime DateTimeUnixEpochStart = new DateTime(2017, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc);
        private static UInt64 CurrentHttpPostNonce { get; set; }
        public static string GetCurrentHttpPostNonce()
        {
            var newHttpPostNonce = Convert.ToUInt64(Math.Round(DateTime.UtcNow.Subtract(DateTimeUnixEpochStart).TotalSeconds, MidpointRounding.AwayFromZero));
            if (newHttpPostNonce > CurrentHttpPostNonce)
            {
                CurrentHttpPostNonce = newHttpPostNonce;
            }
            else
            {
                CurrentHttpPostNonce += 1;
            }

            return CurrentHttpPostNonce.ToString();
        }
        const string ApiCallTemplate = "https://yobit.net/tapi/";
        private static string HashHmac(string message, string secret)
        {
            var keyByte = Encoding.UTF8.GetBytes(secret);
            string sign1 = string.Empty;
            byte[] inputBytes = Encoding.UTF8.GetBytes(message);
            using (var hmac = new HMACSHA512(keyByte))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);

                StringBuilder hex1 = new StringBuilder(hashValue.Length * 2);
                foreach (byte b in hashValue)
                {
                    hex1.AppendFormat("{0:x2}", b);
                }
                sign1 = hex1.ToString();
            }
            return sign1;
        }
        public static string PostString(string method, string postData)
        {
            var nonce = GetCurrentHttpPostNonce();
            string parameters = $"method=" + method + "&nonce=" + nonce;
            string sign1 = HashHmac(parameters, YoubitKey.SecretKey);

            WebRequest Request = (HttpWebRequest)System.Net.WebRequest.Create(ApiCallTemplate);
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Headers.Add("Key", YoubitKey.ApiKey);
            Request.Headers.Add("Sign", sign1);
            Request.ContentLength = parameters.Length;

            byte[] inputBytes = Encoding.UTF8.GetBytes(parameters);
            using (var dataStream = Request.GetRequestStream())
            {
                dataStream.Write(inputBytes, 0, inputBytes.Length);
            }
            WebResponse resp = Request.GetResponse();
            string temp;
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                temp = stream.ReadToEnd();
            }

            return temp;
        }
    }
    public static class ExmoPostRequst
    {
        internal const string ApiUrlHttpsBase = "http://api.exmo.com/v1/{0}";

        public static string PostString(string relativeUrl, Dictionary<string, string> postData)
        {

            postData.Add("nonce", GetCurrentHttpPostNonce());
            var message = postData.ToHttpPostString();

            var sign = Sign(ExmoKey.SecretKey, message);

            var content = new FormUrlEncodedContent(postData);
            content.Headers.Add("Sign", sign);
            content.Headers.Add("Key", ExmoKey.ApiKey);
            string temp = "";
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.PostAsync(string.Format(ApiUrlHttpsBase, relativeUrl), content).Result;
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                string messageex = e.Message;
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, "Exmo", buttons);
                return temp;
            }
        }
        private static string Sign(string key, string message)
        {
            using (HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
            {
                byte[] b = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return ByteToString(b);
            }
        }
        private static string ByteToString(this byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary).ToLowerInvariant();
        }

        private static string ToHttpPostString(this Dictionary<string, string> dictionary)
        {
            var array = (from key in dictionary.Keys
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(dictionary[key])))
                 .ToArray();
            return string.Join("&", array);
        }
        private static string GetCurrentHttpPostNonce()
        {
            var newHttpPostNonce = Convert.ToUInt64(Math.Round((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds, MidpointRounding.AwayFromZero));
            return newHttpPostNonce++.ToString();
        }
    }
    public static class NovaPostRequst
    {
        const string ApiCallTemplate = "https://novaexchange.com/remote/v2/private/";
        private static string HashHmac(string message, string secret)
        {
            Encoding encoding = Encoding.UTF8;
            using (HMACSHA512 hmac = new HMACSHA512(encoding.GetBytes(secret)))
            {
                var msg = encoding.GetBytes(message);
                var hash = hmac.ComputeHash(msg);
                return Convert.ToBase64String(hash);//.ToLower().Replace("-", string.Empty);
            }
        }
        public static string PostString(string relativeUrl, string postData)
        {
            var nonce = DateTime.Now.Ticks;
            var uri = ApiCallTemplate + relativeUrl + '/' + "?nonce=" + nonce;
            var sign = HashHmac(uri, NovaKey.SecretKey);

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("apikey", NovaKey.ApiKey));
            values.Add(new KeyValuePair<string, string>("signature", sign));

            string result = "";
            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                var response = client.PostAsync(uri, content).Result;
                result = response.Content.ReadAsStringAsync().Result;
                // result is {"status": "error", "message": "Auth failed"}
            }
            return result;
        }
    }
    public static class BitbayPostRequst
    {
        private static string ApiCallTemplate = "https://bitbay.net/API/Trading/tradingApi.php";
        private static string HashHmac(string message, string secret)
        {
            var keyByte = Encoding.UTF8.GetBytes(secret);
            using (var hmacsha512 = new HMACSHA512(keyByte))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(message));
                return ByteToString(hmacsha512.Hash);
            }

        }

        public static async Task<string> PostString(string method, Dictionary<string, string> parameters = null)
        {
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }
            var moment = GetNonce();
            parameters.Add("method", method);
            parameters.Add("moment", moment.ToString());

            var queryParams = parameters.ToHttpPostString();


            var signature = HashHmac(queryParams, BitbayKey.SecretKey);
            using (HttpClient _httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Add("API-Key", BitbayKey.ApiKey);
                _httpClient.DefaultRequestHeaders.Add("API-Hash", signature);

                var response = await _httpClient.PostAsync(ApiCallTemplate,
                    new FormUrlEncodedContent(parameters));
                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().Result;

                return json;
            }

        }
        private static string ToHttpPostString(this Dictionary<string, string> dictionary)
        {
            var array = (from key in dictionary.Keys
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(dictionary[key])))
                 .ToArray();
            return string.Join("&", array);
        }
        private static string GetNonce()
        { 

            var date = DateTime.UtcNow;
            return Convert.ToString((int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        }
            private static string ByteToString(byte[] buff)
            {
                string sbinary = "";
                for (int i = 0; i < buff.Length; i++)
                    sbinary += buff[i].ToString("x2");
                return sbinary;
            }
        }
}
