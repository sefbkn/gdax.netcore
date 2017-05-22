using System.Collections.Generic;

namespace Boukenken.Gdax
{
    public class OrderBook
    {
        public long sequence { get; set; }
        public IEnumerable<string[]> bids { get; set; }
        public IEnumerable<string[]> asks { get; set; }
    }
}