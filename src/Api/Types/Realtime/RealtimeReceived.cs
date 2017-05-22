namespace Boukenken.Gdax
{
    public class RealtimeReceived : RealtimeMessage
    {
        public string order_id { get; set; }
        public decimal size { get; set; }
    }
}