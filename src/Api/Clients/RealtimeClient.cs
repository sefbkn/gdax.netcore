using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boukenken.Gdax
{
    public interface IRealtimeClient
    {
        string[] ProductIds { get; }
        Task SubscribeAsync(Action<string> messageReceived);
    }

    public class RealtimeClient
    {
        private Uri _websocketFeedUri;
        
        public RealtimeClient(Uri websocketFeedUri, string[] productIds)
        {
            _websocketFeedUri = websocketFeedUri;
            this.ProductIds = productIds;
        }

        public string[] ProductIds { get; }

        public async Task SubscribeAsync(Action<string> messageReceived)
        {
            if (messageReceived == null)
                throw new ArgumentNullException("onMessageReceived", "Message received callback must not be null.");

            var webSocketClient = new ClientWebSocket();
            var cancellationToken = new CancellationToken();
            await webSocketClient.ConnectAsync(_websocketFeedUri, cancellationToken).ConfigureAwait(false);

            
            if (webSocketClient.State == WebSocketState.Open)
            {
                var requestString = JsonConvert.SerializeObject(new {
                    type = "subscribe",
                    product_ids = ProductIds
                });

                var requestBytes = UTF8Encoding.UTF8.GetBytes(requestString);
                var subscribeRequest = new ArraySegment<byte>(requestBytes);
                var sendCancellationToken = new CancellationToken();
                await webSocketClient.SendAsync(subscribeRequest, WebSocketMessageType.Text, true, sendCancellationToken).ConfigureAwait(false);

                while (webSocketClient.State == WebSocketState.Open)
                {
                    var receiveCancellationToken = new CancellationToken();
                    using(var stream = new MemoryStream(1024))
                    {
                        var receiveBuffer = new ArraySegment<byte>(new byte[1024*8]);
                        WebSocketReceiveResult webSocketReceiveResult;
                        do
                        {
                            webSocketReceiveResult = await webSocketClient.ReceiveAsync(receiveBuffer, receiveCancellationToken).ConfigureAwait(false);
                            await stream.WriteAsync(receiveBuffer.Array, receiveBuffer.Offset, webSocketReceiveResult.Count);
                        } while(!webSocketReceiveResult.EndOfMessage);
                        
                        stream.Seek(0, SeekOrigin.Begin);
                        if (webSocketReceiveResult.MessageType == WebSocketMessageType.Text)
                        {
                            using (var reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                messageReceived(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
        }

        public RealtimeMessage ParseMessage(byte[] message)
        {
            var jsonResponse = Encoding.UTF8.GetString(message, 0, message.Length);
            var jToken = JToken.Parse(jsonResponse);
            var type = jToken["type"]?.Value<string>();

            RealtimeMessage realtimeMessage = null;

            switch (type)
            {
                case "received":
                    realtimeMessage = JsonConvert.DeserializeObject<RealtimeReceived>(jsonResponse);
                    break;
                case "open":
                    realtimeMessage = JsonConvert.DeserializeObject<RealtimeOpen>(jsonResponse);
                    break;
                case "done":
                    realtimeMessage = JsonConvert.DeserializeObject<RealtimeDone>(jsonResponse);
                    break;
                case "match":
                    realtimeMessage = JsonConvert.DeserializeObject<RealtimeMatch>(jsonResponse);
                    break;
                case "change":
                    realtimeMessage = JsonConvert.DeserializeObject<RealtimeChange>(jsonResponse);
                    break;
                case "error":
                    var error = JsonConvert.DeserializeObject<RealtimeError>(jsonResponse);
                    throw new Exception(error.message);
                default:
                    break;
            }

            return realtimeMessage;
        }
    }
}
