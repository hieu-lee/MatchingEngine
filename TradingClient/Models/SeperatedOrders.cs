namespace TradingClient.Models
{
    public class SeperatedOrders
    {
        public string StockId { get; set; }
        public List<Order> BuyOrders { get; set; } = new();
        public List<Order> SellOrders { get; set; } = new();
    }
}