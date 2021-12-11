namespace MatchingEngine.Services
{
    public static class OrderService
    {
        public static Dictionary<string, SortedSet<Order>> SellOrders { get; set; } = new();
        public static Dictionary<string, SortedSet<Order>> BuyOrders { get; set; } = new();
    }
}
