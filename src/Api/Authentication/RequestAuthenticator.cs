using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Boukenken.Gdax
{
    public class RequestAuthenticator : IRequestAuthenticator
    {
        private readonly string _apiKey;
        private readonly string _passphrase;
        private readonly string _secret;

        public RequestAuthenticator(string apiKey, string passphrase, string secret)
        {
            if(string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));
            if(string.IsNullOrEmpty(passphrase)) throw new ArgumentNullException(nameof(passphrase));
            if(string.IsNullOrEmpty(secret)) throw new ArgumentNullException(nameof(secret));

            _apiKey = apiKey;
            _passphrase = passphrase;
            _secret = secret;
        }

        public AuthenticationToken GetAuthenticationToken(ApiRequest request)
        {
            var timestamp = (request.Timestamp).ToString(System.Globalization.CultureInfo.InvariantCulture);
            var signature = ComputeSignature(request);
            return new AuthenticationToken(_apiKey, _passphrase, signature, timestamp);
        }
        
        private string ComputeSignature(ApiRequest request)
        {
            byte[] data = Convert.FromBase64String(_secret);
            var prehash = request.Timestamp + request.HttpMethod.ToString().ToUpper() + request.RequestUrl + request.RequestBody;
            return HashString(prehash, data);
        }

        private string HashString(string str, byte[] secret)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            using (var hmac = new HMACSHA256(secret))
            {
                byte[] hash = hmac.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
