namespace TradingClient.Models
{
    public class UserStock
    {
        public string Id => OwnerId + StockId;
        public string OwnerId { get; set; }
        public string StockId { get; set; }
        public uint Quantity { get; set; } = 0;
        public User Owner { get; set; }
        public Stock Stock { get; set; }
    }
}