namespace TradingClient.Services
{
    public class TransactionService
    {
        private readonly HttpClient _httpClient;

        public TransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync() => await _httpClient.GetFromJsonAsync<List<Transaction>>("api/Transaction/get-all");

        public async Task<List<Transaction>> GetUserBuyTransactionsAsync(string userId) => await _httpClient.GetFromJsonAsync<List<Transaction>>($"api/Transaction/get-buy/{userId}");

        public async Task<List<Transaction>> GetUserSellTransactionsAsync(string userId) => await _httpClient.GetFromJsonAsync<List<Transaction>>($"api/Transaction/get-sell/{userId}");

    }
}
