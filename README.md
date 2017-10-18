# gdax.net
GDAX API Client Library (C# / .NET Core)


# Example usage

            var baseUrl = "https://api.gdax.com/";
            var apiKey = "<api-key>";
            var secret = "<secret>";
            var passphrase = "<passphrase>";

            var requestAuthenticator = new RequestAuthenticator(apiKey, passphrase, secret); 
            var productClient = new ProductClient(baseUrl, requestAuthenticator);
            var productResponse = await productClient.GetProductTickerAsync("BTC-USD");
            
            if(productResponse.StatusCode == HttpStatusCode.OK)
            {
                var ticker = productResponse.Value;
                Console.WriteLine("Price: {0}", ticker.price);
            }
            
