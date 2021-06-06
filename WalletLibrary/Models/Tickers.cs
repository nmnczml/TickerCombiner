using System;
namespace WalletLibrary.Models
{
    public class Tickers
    {

        public string Market;
        public string Symbol;
        public string LastPrice { get; set; }
        public string PriceChangePercent { get; set; }
        public string HighPrice { get; set; }
        public string LowPrice { get; set; }
        public string QuoteVolume { get; set; }                       

        public Tickers()
        {
        }
    }
}
