using System;
using System.Collections.Generic;
using BtcTurk.Net;
using WalletLibrary.Models;

namespace WalletLibrary
{
    public class BTCTurkWallet : IWallet
    {
        /* BtcTurkClient Object */
        BtcTurkClient m_ApiClient = new BtcTurkClient();

        
        public BTCTurkWallet(string apiKey, string secret)
        {
            m_ApiClient.SetApiCredentials(apiKey, secret);
                  
        }

        public List<AvailableMarketPairs> GetBalance()
        {


            var userBalances = m_ApiClient.GetBalances();


          

            List<AvailableMarketPairs> userBalanceList = new List<AvailableMarketPairs>();
            foreach (var userBalance in userBalances.Data)
            {

                AvailableMarketPairs pair = new AvailableMarketPairs();
                pair.Pair = userBalance.AssetName;
                pair.Free = userBalance.Free.ToString();
                pair.Locked = userBalance.Locked.ToString();
                pair.Total = Convert.ToString(userBalance.Free + userBalance.Locked);
                userBalanceList.Add(pair);
            }
            return userBalanceList;
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
