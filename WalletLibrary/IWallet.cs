using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletLibrary.Models;

namespace WalletLibrary
{
    public interface IWallet
    {
        public  List<AvailableMarketPairs>  GetBalance();
        public bool ConfirmKey(string market, string apiKey, string secret);
        public List<Tickers> GetTickers(List<string> symbolList);

    }
}
