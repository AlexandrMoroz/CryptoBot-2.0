using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptobot.Interfesse
{

    public static class AccseptCoins
    {
        static public char SPLITER = '-';
        public static List<KeyValuePair<string, string>> GetCoins()
        {
            List<KeyValuePair<string, string>> tp = new List<KeyValuePair<string, string>>();
            using (StreamReader stream = new StreamReader(
                   "prox/coins.txt"))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    tp.Add(new KeyValuePair<string, string>(line.Split(SPLITER)[0], line.Split(SPLITER)[1]));
                }
            }
            return tp;
        }
    }
}
