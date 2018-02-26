using System;

namespace Boukenken.Gdax
{
    public class LedgerEntryDetail
    {
        public Guid order_id { get; set; }
        public string trade_id { get; set; }
        public string product_id { get; set; }
    }
}