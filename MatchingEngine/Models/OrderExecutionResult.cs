namespace MatchingEngine.Models
{
    public class OrderExecutionResult
    {
        public Dictionary<string, double> BalanceChanges { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
        public List<Order> BuyOrdersLeft { get; set; } = new();
        public List<Order> SellOrdersLeft { get; set; } = new();
    }
}