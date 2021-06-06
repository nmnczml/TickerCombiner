using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Binance.NetCore;
using WalletLibrary.Models;

namespace WalletLibrary
{
    public class BinanceWallet: IWallet
    {
        BinanceApiClient m_Client;
        public BinanceWallet(string apiKey, string secret)
        {

            m_Client = new BinanceApiClient(apiKey, secret);
            
        }

        public List<AvailableMarketPairs> GetBalance()
        {
            var userBalances = m_Client.GetBalance();
            List<AvailableMarketPairs> userBalanceList = new List<AvailableMarketPairs>();
            foreach (var userBalance in userBalances.balances)
            {
                if (userBalance.free > 0 || userBalance.locked > 0)
                {
                    AvailableMarketPairs pair = new AvailableMarketPairs();
                    pair.Market = "Binance";
                    pair.Pair = userBalance.asset;
                    pair.Free = userBalance.free.ToString();
                    pair.Locked = userBalance.locked.ToString();
                    pair.Total = Convert.ToString(userBalance.free + userBalance.locked);
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
 
            var list = m_Client.GetTickers();

            List<string> binanceSymbolList = new List<string>();

            foreach (var symbol in symbolList)
            {

                string market = symbol.Split('@')[0];
                string coin = symbol.Split('@')[1].ToUpper();
                string pair = symbol.Split('@')[2].ToUpper();
                if (market.Equals("Binance"))
                {
                    string modifiedSymbol = coin + pair;
                    binanceSymbolList.Add(modifiedSymbol);
                }
            }

            List<Tickers> result = new List<Tickers>();
            foreach (var tick in list)
            {
                if (binanceSymbolList.Contains(tick.Symbol))
                {
                    Tickers rTicker = new Tickers();
                    rTicker.Symbol = tick.Symbol;
                    rTicker.LastPrice = tick.Price.ToString();
                    rTicker.Market = "Binance";
                    result.Add(rTicker);
                }
            }

            return result;

        }

          
    }
}
