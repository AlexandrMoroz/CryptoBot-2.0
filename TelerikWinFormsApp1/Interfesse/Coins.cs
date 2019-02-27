using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cryptobot.Models;
using Cryptobot.Models.InfoModels;
using Cryptobot.Stocks;

namespace Cryptobot
{
    public class TransformWithdrow
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Wallet { get; set; }
        public DateTime Date { get; set; }
        public TransformWithdrow()
        {

        }
        public TransformWithdrow(int id, string currencyPair, string address, decimal amount, DateTime date)
        {
            Id = id;
            Amount = amount;
            Currency = currencyPair;
            Wallet = address;
            Date = date;
        }

    }

    public class TransformBallans
    {

        public decimal Available { get; set; }
        public decimal OnOrders { get; set; }

        public TransformBallans() { }
        public TransformBallans(decimal available, decimal onorders)
        { 
            Available = available;
            OnOrders = onorders;
        }
        public TransformBallans(PoloniexWalletField arg)
        {
            Available = arg.available;
            OnOrders = arg.onOrders;
        }
    }

    





}
