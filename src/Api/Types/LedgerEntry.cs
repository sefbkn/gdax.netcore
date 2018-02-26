using System;

namespace Boukenken.Gdax
{
    public class LedgerEntry
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public decimal amount { get; set; }
        public decimal balance { get; set; }
        public string type { get; set; }
        public LedgerEntryDetail details { get; set; }
    }
}