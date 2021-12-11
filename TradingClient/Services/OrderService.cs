namespace TradingClient.Services
{
    public class OrderService
    {
        static string url = "https://localhost:7007/order";
        HubConnection _connection = new HubConnectionBuilder().WithUrl(url).Build();
        public bool isConnected = false;

        public OrderService()
        {
            _connection.Closed += async (s) =>
            {
                isConnected = false;
                await _connection.StartAsync();
                isConnected = true;
            };
        }

        public async Task ConnectAsync()
        {
            await _connection.StartAsync();
            isConnected = true;
        }

        public static OrderService OrderServiceBuildAsync()
        {
            var res = new OrderService();
            res.ConnectAsync().ContinueWith(task =>
            {
            });
            return res;
        }
    }
}
