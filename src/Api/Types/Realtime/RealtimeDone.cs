namespace Boukenken.Gdax
{
    public class RealtimeDone : RealtimeMessage
    {
        public string order_id { get; set; }
        public decimal remaining_size { get; set; }
        public string reason { get; set; }
    }
}