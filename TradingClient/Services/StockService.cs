namespace TradingClient.Services
{
    public class StockService
    {
        private readonly HttpClient _httpClient;

        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Stock>> GetAllStocksAsync() => await _httpClient.GetFromJsonAsync<List<Stock>>("api/Stock/get-all");

        public async Task<Stock> GetStockAsynct(string id) => await _httpClient.GetFromJsonAsync<Stock>($"api/Stock/get-id/{id}");

        public async Task<OrderBook> GetStockOrderBook(string id) => await _httpClient.GetFromJsonAsync<OrderBook>($"api/Stock/get-order-book/{id}");

        public async Task AddStockAsync(Stock stock)
        {
            await _httpClient.PostAsJsonAsync("api/Stock/create", stock);
        }

        public async Task AddStockToUserAsync(string stockId, string userId)
        {
            await _httpClient.PostAsJsonAsync($"api/Stock/add-user-stock/{stockId}", userId);
        }
    }
}
