using System;
namespace WalletLibrary.Models
{
    public class MarketApiKey
    {
        public enum MarketName
        {
            Binance,
            GateIO,
            BTCTurk
        }
        public string ApiLabel { get; set; }
        public string ApiKey { get; set; }
        public string Secret { get; set; }
        public MarketName Market { get; set; }

        public MarketApiKey()
        {

        }
    }
}
