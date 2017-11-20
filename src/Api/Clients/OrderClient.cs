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
        Task<ApiResponse<Order>> PlaceOrderAsync(string side, string productId, decimal size, decimal price, string type, bool postOnly, string cancelAfter = null, string timeInForce = null);
    }

	public class OrderClient : GdaxClient
    {
        public OrderClient(string baseUrl, RequestAuthenticator authenticator)
            : base(baseUrl, authenticator)
        {
        }

		public async Task<ApiResponse<Order>> PlaceOrderAsync(string side, string productId, decimal size, decimal price, string type, bool postOnly, string cancelAfter = null, string timeInForce = null)
		{
			return await this.GetResponseAsync<Order>(
				new ApiRequest(HttpMethod.Post, "/orders", Serialize(new
				{
					size = size,
					side = side,
					type = type,
					price = price,
					product_id = productId,
					post_only = postOnly,
					cancel_after = cancelAfter,
					time_in_force = timeInForce
				}))
			);
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
					post_only = false,
                    cancel_after = cancelAfter,
                    time_in_force = timeInForce
                }))
            );
        }

        public async Task<ApiResponse<IEnumerable<Order>>> GetOpenOrdersAsync()
        {
            return await this.GetResponseAsync<IEnumerable<Order>>(
                new ApiRequest(HttpMethod.Get, "/orders?status=all")
            );
        }

		public async Task<ApiResponse<Order>> GetOpenOrdersAsync(string Id)
		{
			return await this.GetResponseAsync<Order>(
				new ApiRequest(HttpMethod.Get, "/orders/" + Id)
			);
		}

		public async Task<ApiResponse<IEnumerable<Guid>>> CancelOpenOrdersAsync(string productId = null)
        {
            return await this.GetResponseAsync<IEnumerable<Guid>>(
                new ApiRequest(HttpMethod.Delete, "/orders" + (productId == null ? "" : $"?product_id={productId}"))
            );
        }
    }
}