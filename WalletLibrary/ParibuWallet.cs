using System;
using System.Collections.Generic;
using Paribu.Net;
using Paribu.Net.CoreObjects;
using WalletLibrary.Models;

namespace WalletLibrary
{
    public class ParibuWallet : IWallet
    {
        // Rest Api Client
        private ParibuClientOptions options = new ParibuClientOptions();
        private ParibuClient m_Client;
       
         
        public ParibuWallet(string apiKey, string secret)
        {
            options.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials(apiKey, secret);
            m_Client = new ParibuClient(options);
          
        }


        public List<AvailableMarketPairs> GetBalance()
        {
            return null;
        }

        public bool ConfirmKey(string market, string apiKey, string secret)
        {
            return false;
        }

        public List<Tickers> GetTickers(List<string> symbolList)
        {
            return null;
        }
    }
}
