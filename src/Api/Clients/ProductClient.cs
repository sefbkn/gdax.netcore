using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Boukenken.Gdax
{
    public interface IProductClient
    {
        Task<ApiResponse<IEnumerable<Product>>> GetProductsAsync();
        Task<ApiResponse<ProductTicker>> GetProductTickerAsync(string productId);
        Task<ApiResponse<OrderBook>> GetOrderBookAsync(string productId, int level = 1);
    }

    public class ProductClient : GdaxClient, IProductClient
    {
        public ProductClient(string baseUrl, RequestAuthenticator authenticator)
            : base(baseUrl, authenticator)
        {
        }

        public async Task<ApiResponse<IEnumerable<Product>>> GetProductsAsync()
        {
            return await this.GetResponseAsync<IEnumerable<Product>>(
                new ApiRequest(HttpMethod.Get, "/products")
            );
        }

        public async Task<ApiResponse<ProductTicker>> GetProductTickerAsync(string productId)
        {
            var response = await this.GetResponseAsync<ProductTicker>(
                new ApiRequest(HttpMethod.Get, $"/products/{productId}/ticker")
            );

            if(response.Value != null)
                response.Value.product_id = productId;

            return response;
        }

        public async Task<ApiResponse<OrderBook>> GetOrderBookAsync(string productId, int level = 1)
        {
            return await this.GetResponseAsync<OrderBook>(
                new ApiRequest(HttpMethod.Get, $"/products/{productId}/book?level={level}")
            );
        }
    }
}