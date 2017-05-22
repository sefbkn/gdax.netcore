using System;

namespace Boukenken.Gdax
{
    public class RealtimeChange : RealtimeMessage
    {
        public string order_id { get; set; }
        public DateTime time { get; set; }
        public decimal new_size { get; set; }
        public decimal old_size { get; set; }
    }

}