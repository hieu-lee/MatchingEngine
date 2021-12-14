namespace TradingClient.Models
{
    public class OrderBook
    {
        public string StockId { get; set; }
        public SortedSet<Order> BuyOrder { get; set; } = new();
        public SortedSet<Order> SellOrder { get; set; } = new();
    }
}