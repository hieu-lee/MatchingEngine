namespace MatchingEngine.Models
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime Time = DateTime.Now.ToUniversalTime();

        [ForeignKey("Buyer")]
        public string BuyerId { get; set; }
        [ForeignKey("Seller")]
        public string SellerId { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public uint Quantity { get; set; }
        [ForeignKey("Stock")]
        public string StockId { get; set; }
        public Stock Stock { get; set; }
        public override string ToString()
        {
            return $"{BuyerId} buy {Quantity} {StockId} at price {Price} from {SellerId}";
        }
    }
}
