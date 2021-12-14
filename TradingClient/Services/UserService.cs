
namespace TradingClient.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> GetUserAsync(string Id) => await _httpClient.GetFromJsonAsync<User>($"api/User/get-id/{Id}");

        public async Task<User> GetUserByEmailAsync(string email) => await _httpClient.GetFromJsonAsync<User>($"api/User/get-email/{email}");

        public async Task CreateUserAsync(User user)
        {
            try
            {
                await _httpClient.PostAsJsonAsync("api/User/create-user", user);
            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            await _httpClient.PutAsJsonAsync("api/User/update", user);
        }
    }
}
