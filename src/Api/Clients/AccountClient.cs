using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Boukenken.Gdax
{
    public interface IAccountClient
    {
        Task<ApiResponse<List<Account>>> ListAccountsAsync(Guid? accountId = null);
        Task<ApiResponse<List<LedgerEntry>>> ListAccountHistoryAsync(Guid accountId, string cbAfter = "");
    }

    public class AccountClient : GdaxClient, IAccountClient
    {
        public AccountClient(string baseUrl, RequestAuthenticator authenticator)
            : base(baseUrl, authenticator)
        {
        }

        public async Task<ApiResponse<List<Account>>> ListAccountsAsync(Guid? accountId = null)
        {
            return await this.GetResponseAsync<List<Account>>(
                new ApiRequest(HttpMethod.Get, $"/accounts/{accountId?.ToString().ToLower()}")
            );
        }

        public async Task<ApiResponse<List<LedgerEntry>>> ListAccountHistoryAsync(Guid accountId, string cbAfter = "")
        {
            if (!(cbAfter == ""))
                cbAfter = "?after=" + cbAfter;
            return await this.GetResponseAsync<List<LedgerEntry>>(
                new ApiRequest(HttpMethod.Get, $"/accounts/{accountId.ToString().ToLower()}/ledger{cbAfter}")
            );
        }
    }
}