namespace Boukenken.Gdax
{
    public interface IRequestAuthenticator
    {
        AuthenticationToken GetAuthenticationToken(ApiRequest request);
    }
}
