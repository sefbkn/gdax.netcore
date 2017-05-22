using System;
using System.Net.Http;

namespace Boukenken.Gdax
{
    public class ApiRequest
    {
        public ApiRequest(HttpMethod httpMethod, string relativePath, string requestBody = null)
        {
            this.HttpMethod = httpMethod;
            this.RequestUrl = relativePath;
            this.RequestBody = requestBody;
            this.Timestamp = DateTime.UtcNow.ToUnixTimestamp();
        }

        public double Timestamp { get; private set; }
        public HttpMethod HttpMethod { get; private set; }
        public string RequestUrl { get; }
        public string RequestBody { get; }

        public bool IsExpired
        {
            get { return (GetCurrentUnixTimeStamp() - Timestamp) >= 30; } 
        }

        protected virtual double GetCurrentUnixTimeStamp()
        {
            return DateTime.UtcNow.ToUnixTimestamp();
        }
    }
}