using System;
using System.Collections.Generic;
using WalletLibrary.Models;

namespace WalletLibrary
{
    public class CombinedWallet:IWallet
    {
     
        
        public List<MarketApiKey> ApiKeys = new List<MarketApiKey>();
        public CombinedWallet()
        {
        }

        public List<AvailableMarketPairs> GetBalance()
        {

            List<AvailableMarketPairs> result = new List<AvailableMarketPairs>();

            foreach (var market in ApiKeys)
            {
                IWallet wallet = WalletFactory.CreateWallet(market);

                result.AddRange(wallet.GetBalance());

            }

            return result;

        }

        public bool ConfirmKey(string market, string apiKey, string secret)
        {
            MarketApiKey marketDef = new MarketApiKey();
            marketDef.ApiKey = apiKey;
            marketDef.Secret = secret;
            Enum.TryParse( market, out MarketApiKey.MarketName def);
            marketDef.Market = def;
            IWallet wallet = WalletFactory.CreateWallet(marketDef);

            return wallet.ConfirmKey(  market,   apiKey,   secret);
        }

        public List<Tickers> GetTickers(List<string> symbolList)
        {
            List<Tickers> result = new List<Tickers>();

            foreach (var market in ApiKeys)
            {
                IWallet wallet = WalletFactory.CreateWallet(market);

                result.AddRange(wallet.GetTickers(symbolList));

            }

            return result;
        }


    }
}
