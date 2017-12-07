using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boukenken.Gdax
{
    public interface IProductClient
    {
        Task<ApiResponse<IEnumerable<Product>>> GetProductsAsync();
        Task<ApiResponse<ProductTicker>> GetProductTickerAsync(string productId);
        Task<ApiResponse<OrderBook>> GetOrderBookAsync(string productId, int level = 1);
		Task<List<Candle>> GetHistoricRatesAsync(string i_product_id, DateTimeOffset i_start, DateTimeOffset i_end, TimeSpan i_granularity);
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
			var response = await this.GetResponseAsync<ProductTicker>(
				new ApiRequest(HttpMethod.Get, $"/products/{productId}/ticker")
			);

			if (response.Value != null)
				response.Value.product_id = productId;

			return response;
		}

		public async Task<ApiResponse<OrderBook>> GetOrderBookAsync(string productId, int level = 1)
		{
			return await this.GetResponseAsync<OrderBook>(
				new ApiRequest(HttpMethod.Get, $"/products/{productId}/book?level={level}")
			);
		}

		// Due to exchange time limitations on API method iteration intervals, invoke this API end point no more than 1 per second.  Maximum list size 
		// returned by the API is 200 candles of size granularity.  In other words the ((i_end - i_start) / i_granularity) < 200 must be true.
		// For multiple iterations where the number of candles required is greater than 200, aggregate the returned List into a Master List. 
		//
		// Example aggregation use:
		//
		//
		//		static List<Candle> CandleRecords = new List<Candle>();
		//
		//		static Timer ExchangeCommunicatonChokeTimer;
		//		static bool AllowCommunication = true;
		//
		//		ExchangeCommunicatonChokeTimer = new Timer(1000);                                   // order status timer 1 second intervals
		//		ExchangeCommunicatonChokeTimer.AutoReset = true;                                    
		//		ExchangeCommunicatonChokeTimer.Enabled = true;
		//		ExchangeCommunicatonChokeTimer.Elapsed += (sender, e) => AllowCommunication = true;
		//		//
		//		// initialize start, end, granularity, set notDone = true
		//		//
		//		while(notDone)
		//		{
		//			if (AllowCommunication)
		//			{
		//				AllowCommunication  = false;
		//				List<Candle> CandleData = await productClient.GetHistoricRatesAsync(start, end, granularity);
		//				var result = CandleRecords.Concat(CandleData);
		//				CandleRecords = result.ToList();
		//				//
		//				// calculate new start, end, and notDone
		//				//
		//			}
		//		}
		public async Task<List<Candle>> GetHistoricRatesAsync(string i_product_id, DateTimeOffset i_start, DateTimeOffset i_end, TimeSpan i_granularity)
		{
			var CandleData =  await this.GetResponseAsync<JArray>(
					new ApiRequest(HttpMethod.Get, $"/products/{i_product_id}/candles?start={i_start.ToString("yyyy-MM-ddTHH:mm:00.00000Z")}&end={i_end.ToString("yyyy-MM-ddTHH:mm:00.00000Z")}&granularity={i_granularity.TotalSeconds}")
					);
			Candle c = new Candle();
			List<Candle> cl = new List<Candle>();
			JArray x = CandleData.Value;
			int index = x.Count;
			while (index-- > 0)
			{
				c.time = (long)x[index][0];
				c.low = (decimal)x[index][1];
				c.high = (decimal)x[index][2];
				c.open = (decimal)x[index][3];
				c.close = (decimal)x[index][4];
				c.volume = (decimal)x[index][5];
				cl.Add(c);
			}

			return cl;
		}
	}
}