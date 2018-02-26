using System;

namespace Boukenken.Gdax
{
    public class Order
    {
        public Guid id { get; set; }
        public decimal price { get; set; }
        public decimal size { get; set; }
        public string product_id { get; set; }
        public string side { get; set; }
        public string stp { get; set; }
        public string type { get; set; }
        public string time_in_force { get; set; }
        public string post_only { get; set; }
        public string created_at { get; set; }
        public string done_at { get; set; }
        public decimal fill_fees { get; set; }
        public decimal filled_size { get; set; }
        public decimal executed_value { get; set; }
        public string status { get; set; }
        public bool settled { get; set; }
    }
}