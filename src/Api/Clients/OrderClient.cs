using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Boukenken.Gdax
{
    public interface IOrderClient
    {
        Task<ApiResponse<IEnumerable<Order>>> GetOpenOrdersAsync();
        Task<ApiResponse<IEnumerable<Guid>>> CancelOpenOrdersAsync(string productId = null);
        Task<ApiResponse<Order>> PlaceOrderAsync(string side, string productId, decimal size, decimal price, string type, string cancelAfter = null, string timeInForce = null);
        Task<List<Candle>> GetHistoricRatesAsync(string productId, DateTimeOffset start, DateTimeOffset end, TimeSpan granularity);

    }

    public class OrderClient : GdaxClient, IOrderClient
    {
        public OrderClient(string baseUrl, RequestAuthenticator authenticator)
            : base(baseUrl, authenticator)
        {
        }

        public async Task<ApiResponse<Order>> PlaceOrderAsync(string side, string productId, decimal size, decimal price, string type, string cancelAfter = null, string timeInForce = null)
        {
            return await this.GetResponseAsync<Order>(
                new ApiRequest(HttpMethod.Post, "/orders", Serialize(new {
                    size = size,
                    side = side,
                    type = type,
                    price = price,
                    product_id = productId,
                    cancel_after = cancelAfter,
                    time_in_force = timeInForce
                }))
            );
        }

        public async Task<ApiResponse<IEnumerable<Order>>> GetOpenOrdersAsync()
        {
            return await this.GetResponseAsync<IEnumerable<Order>>(
                new ApiRequest(HttpMethod.Get, "/orders")
            );
        }

        public async Task<ApiResponse<IEnumerable<Guid>>> CancelOpenOrdersAsync(string productId = null)
        {
            return await this.GetResponseAsync<IEnumerable<Guid>>(
                new ApiRequest(HttpMethod.Delete, "/orders" + (productId == null ? "" : $"?product_id={productId}"))
            );
        }

        public async Task<List<Candle>> GetHistoricRatesAsync(string productId, DateTimeOffset start, DateTimeOffset end, TimeSpan granularity)
        {
            var CandleData =  await this.GetResponseAsync<JArray>(
 					new ApiRequest(HttpMethod.Get, $"/products/{productId}/candles?start={start.ToString("yyyy-MM-ddTHH:mm:00.00000Z")}&end={end.ToString("yyyy-MM-ddTHH:mm:00.00000Z")}&granularity={granularity.TotalSeconds}")
 					);

            Candle candle = new Candle();
            List<Candle> candles = new List<Candle>();
            JArray candlesJsonArray = CandleData.Value;
            int index = candlesJsonArray.Count;
            while (index-- > 0)
            {
                candle.time = (long)candlesJsonArray[index][0];
                candle.low = (decimal)candlesJsonArray[index][1];
                candle.high = (decimal)candlesJsonArray[index][2];
                candle.open = (decimal)candlesJsonArray[index][3];
                candle.close = (decimal)candlesJsonArray[index][4];
                candle.volume = (decimal)candlesJsonArray[index][5];
                candles.Add(candle);
            }

 			return candles;        }
    }
}