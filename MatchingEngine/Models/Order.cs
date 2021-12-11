namespace MatchingEngine.Models
{
    public class Order : IComparable<Order>
    {
        [Key]
        public string Id = Guid.NewGuid().ToString();
        public DateTime OrderTime { get; set; } = DateTime.UtcNow;
        [Required]
        public OrderType OrderType { get; set; }
        [Required]
        public string StockId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public uint Quantity { get; set; }
        [Required]
        public double Price { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Order;
            return Id == other.Id;
        }

        public int CompareTo(Order other)
        {
            var res = Price.CompareTo(other.Price);
            if (res == 0) 
            {
                res = Quantity.CompareTo(other.Quantity);
                if (res == 0)
                {
                    res = OrderTime.CompareTo(other.OrderTime);
                }
                if (res == 0)
                {
                    res = Id.CompareTo(other.Id);
                }
            }
            return res;
        }

        public override string ToString()
        {
            if (OrderType == OrderType.BUY)
            {
                return $"{CustomerId} wants to buy {Quantity} {StockId} at price {Price}";
            }
            return $"{CustomerId} wants to sell {Quantity} {StockId} at price {Price}";
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
