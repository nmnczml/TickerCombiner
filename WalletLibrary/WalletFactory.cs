using System;
using WalletLibrary.Models;

namespace WalletLibrary
{
    public class WalletFactory
    {
        public WalletFactory()
        {
        }

        public static IWallet CreateWallet(MarketApiKey market)
        {
            IWallet wallet = null;
            if (market.Market == MarketApiKey.MarketName.Binance)
            {
                wallet = new BinanceWallet(market.ApiKey, market.Secret);
            }
            else if (market.Market == MarketApiKey.MarketName.GateIO)
            {
                wallet = new GateIoWallet(market.ApiKey, market.Secret);
            }
            else if (market.Market == MarketApiKey.MarketName.BTCTurk)
            {
                wallet = new BTCTurkWallet(market.ApiKey, market.Secret);
            }

            return wallet;
        }
    }
}
