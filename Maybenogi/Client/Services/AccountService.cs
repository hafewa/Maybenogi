using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Maybenogi.Shared.Model;

namespace Maybenogi.Client.Services
{
    internal interface IAccountService
    {
        Task Register(CreateNexonAccount account);
        Task<IList<NexonAccount>> GetAllAccounts();
        Task DeleteAccount(long uid);
        Task<NexonAccount> GetAccount(long uid);
    }

    internal class AccountService : IAccountService
    {
        private HttpClient _httpClient;
        
        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Register(CreateNexonAccount account)
        {
            await _httpClient.PostAsJsonAsync<CreateNexonAccount>("api/accounts", account);
        }

        public async Task<IList<NexonAccount>> GetAllAccounts()
        {
            var result = await _httpClient.GetFromJsonAsync<NexonAccount[]>("api/accounts");
            return result;
        }

        public async Task DeleteAccount(long uid)
        {
            throw new System.NotImplementedException();
        }

        public async Task<NexonAccount> GetAccount(long uid)
        {
            return await _httpClient.GetFromJsonAsync<NexonAccount>($"api/accounts/{uid}");
        }
    }
}
