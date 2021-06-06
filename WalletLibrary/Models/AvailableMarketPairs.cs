using System;
namespace WalletLibrary.Models
{
    public class AvailableMarketPairs
    {
        public AvailableMarketPairs()
        {

        }

        public string Id { get; set; }

        public string Market { get; set; }
        public string Pair { get; set; }
        public string Free { get; set; }
        public string Locked { get; set; }
        public string Total { get; set; }
        public string Icon { get; set; }


    }
}
