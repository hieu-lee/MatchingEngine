namespace MatchingEngine.Models
{
    public class UserStock
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        [ForeignKey("Stock")]
        public string StockId { get; set; }
        public uint Quantity { get; set; } = 0;
        public User Owner { get; set; }
        public Stock Stock { get; set; }

        public UserStock()
        {
            Id = OwnerId + StockId;
        }
    }
}
