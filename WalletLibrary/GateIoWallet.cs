using System;
using System.Collections.Generic;
using Io.Gate.GateApi.Api;
using Io.Gate.GateApi.Client;
using Io.Gate.GateApi.Model;
using WalletLibrary.Models;

namespace WalletLibrary
{
    public class GateIoWallet : IWallet
    {
        Configuration config = new Configuration();
        public GateIoWallet(string apiKey, string secret)
        {
          
            config.BasePath = "https://api.gateio.ws/api/v4";
            config.ApiV4Key = apiKey;
            config.ApiV4Secret = secret;
        }

        public List<AvailableMarketPairs> GetBalance()
        {
 
            Io.Gate.GateApi.Api.SpotApi spotAccounts = new SpotApi(config);
            var userBalances = spotAccounts.ListSpotAccounts();

            List<AvailableMarketPairs> userBalanceList = new List<AvailableMarketPairs>();
            foreach (var userBalance in userBalances)
            {
                if (Convert.ToDecimal(userBalance.Available) > 0 || Convert.ToDecimal(userBalance.Locked) > 0)
                {

                    AvailableMarketPairs pair = new AvailableMarketPairs();
                    pair.Market = "GateIO";
                    pair.Pair = userBalance.Currency;
                    pair.Free = userBalance.Available;
                    pair.Locked = userBalance.Locked;
                    pair.Total = Convert.ToString(Convert.ToDecimal( userBalance.Available) + Convert.ToDecimal(userBalance.Locked));
                    userBalanceList.Add(pair);
                }
            }
            return userBalanceList;
        }

        public bool ConfirmKey(string market, string apiKey, string secret)
        {
            return true;
        }

        public List<Tickers> GetTickers(List<string> symbolList)
        {
            Io.Gate.GateApi.Api.SpotApi spotAccounts = new SpotApi(config);
            var list = spotAccounts.ListTickers();


            List<string> gateIOSymbols = new List<string>();

            foreach (var symbol in symbolList)
            {

                string market = symbol.Split('@')[0];
                string coin = symbol.Split('@')[1].ToUpper();
                string pair = symbol.Split('@')[2].ToUpper();
                if (market.Equals("GateIO"))
                {
                    string modifiedSymbol = coin + "_" + pair;
                    gateIOSymbols.Add(modifiedSymbol);
                }
            }

            List<Tickers> result = new List<Tickers>();
            foreach (var tick in list)
            {
                if (gateIOSymbols.Contains(tick.CurrencyPair))
                {
                    Tickers rTicker = new Tickers();
                    rTicker.Symbol = tick.CurrencyPair.Replace("_","");
                    rTicker.LastPrice = tick.Last.ToString();
                    rTicker.Market = "GateIO";
                    result.Add(rTicker);
                }
            }

            return result;

        }

    }
}
