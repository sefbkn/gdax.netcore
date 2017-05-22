using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Boukenken.Gdax
{
    public class ApiResponse
    {
        public KeyValuePair<string, IEnumerable<string>>[] Headers { get; }
        public HttpStatusCode StatusCode { get; }
        public string Content { get; }

        public ApiResponse(
            KeyValuePair<string, IEnumerable<string>>[] headers,
            HttpStatusCode statusCode, 
            string content)
        {
            this.Headers = headers.ToArray();
            this.StatusCode = statusCode;
            this.Content = content;
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Value { get; }

        public ApiResponse(
            KeyValuePair<string, IEnumerable<string>>[] headers,
            HttpStatusCode statusCode, 
            string content,
            T typedContent) : base(headers, statusCode, content)
        {
            this.Value = typedContent;
        }
    }
}