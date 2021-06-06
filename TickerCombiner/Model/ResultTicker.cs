using System;
namespace TickerCombiner.Model
{
    public class ResultTicker
    {

     //   {"e":"24hrMiniTicker",
     //   "E":1621004640676,"s":"BNBUSDT","c":"609.38000000","o":"599.21000000","h":"612.00000000","l":"526.60000000","v":"3776448.92320000","q":"2171494741.27606500"}


    /// <summary>
    /// Market
    /// </summary>
    public string m { get; set; }


        /// <summary>
        /// Epoch
        /// </summary>
        public long E { get; set; }

        /// <summary>
        /// symbol
        /// </summary>
        public string s { get; set; }


        /// <summary>
        /// Event Type
        /// </summary>
        public string e { get; set; }

        /// <summary>
        /// Current day's close price
        /// </summary>
        public string c { get; set; }


        /// <summary>
        /// High price
        /// </summary>
        public string h { get; set; }

        /// <summary>
        ///  Low price
        /// </summary>
        public string l { get; set; }

        /// <summary>
        /// Total traded base asset volume
        /// </summary>
        public string v { get; set; }

        /// <summary>
        /// Total traded quote asset volume
        /// </summary>
        public string q { get; set; }
    }


}
