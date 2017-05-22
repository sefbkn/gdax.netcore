namespace Boukenken.Gdax
{
    public class AuthenticationToken
    {
        public string Key { get; set; }
        public string Signature { get; set; }
        public string Timestamp { get; set; }
        public string Passphrase { get; set; }

        public AuthenticationToken(string key, string passphrase, string signature, string timestamp)
        {
            Key = key;
            Passphrase = passphrase;
            Signature = signature;
            Timestamp = timestamp;
        }
    }
}