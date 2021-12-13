namespace MatchingEngine.Services
{
    public class TransactionService
    {
        private readonly AppDbContext dbContext;

        public TransactionService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await dbContext.Transactions.ToListAsync();
        }

        public async Task<List<Transaction>> GetUserTransactionsAsync(string userId)
        {
            return await dbContext.Transactions.Where(s => (s.BuyerId == userId) || (s.SellerId == userId)).ToListAsync();
        }

        public async Task<List<Transaction>> GetUserBuyTransactionsAsync(string userId)
        {
            return await dbContext.Transactions.Where(s => s.BuyerId == userId).ToListAsync();
        }

        public async Task<List<Transaction>> GetUserSellTransactionsAsync(string userId)
        {
            return await dbContext.Transactions.Where(s => s.SellerId == userId).ToListAsync();
        }

        public async Task<Result> AddTransactionAsync(Transaction transaction)
        {
            var stockId = transaction.StockId;
            var quantity = transaction.Quantity;
            var buyerStock = transaction.BuyerId + stockId;
            var sellerStock = transaction.SellerId + stockId;
            var balanceChange = Math.Round(transaction.Price * quantity, 2);
            var t1 = dbContext.Users.Where(s => s.Id == transaction.BuyerId).FirstOrDefaultAsync();
            var t2 = dbContext.Users.Where(s => s.Id == transaction.SellerId).FirstOrDefaultAsync();
            var t3 = dbContext.UserStocks.Where(s => s.Id == buyerStock).FirstOrDefaultAsync();
            var t4 = dbContext.UserStocks.Where(s => s.Id == sellerStock).FirstOrDefaultAsync();
            var buyer = await t1;
            var seller = await t2;
            buyer.Balance -= balanceChange;
            seller.Balance += balanceChange;
            buyer.Balance = Math.Round(buyer.Balance, 2);
            seller.Balance = Math.Round(seller.Balance, 2);
            var buyerStockBalance = await t3;
            var sellerStockBalance = await t4;
            if (buyerStockBalance is null)
            {
                buyerStockBalance = new()
                {
                    StockId = stockId,
                    OwnerId = buyer.Id,
                    Quantity = 0
                };
                dbContext.Add(buyerStockBalance);
                await dbContext.SaveChangesAsync();
            }
            buyerStockBalance.Quantity += quantity;
            sellerStockBalance.Quantity -= quantity;
            dbContext.UserStocks.Update(buyerStockBalance);
            dbContext.UserStocks.Update(sellerStockBalance);
            dbContext.Users.Update(buyer);
            dbContext.Users.Update(seller);
            await dbContext.AddAsync(transaction);
            await dbContext.SaveChangesAsync();
            return new();
        }
    }
}
