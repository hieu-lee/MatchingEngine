namespace MatchingEngine.Services
{
    public class StockService
    {
        private readonly AppDbContext dbContext;

        public StockService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await dbContext.Stocks.ToListAsync();
        }

        public async Task<Stock> GetStockAsync(string id)
        {
            return await dbContext.Stocks.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public OrderBook GetStockOrderBook(string id)
        {
            if (OrderService.BuyOrders.ContainsKey(id))
            {
                return new()
                {
                    BuyOrder = OrderService.BuyOrders[id],
                    SellOrder = OrderService.SellOrders[id]
                };
            }
            return null;
        }

        public async Task<Result> AddStockAsync(Stock stock)
        {
            var tmp = await dbContext.Stocks.Where(s => s.Id == stock.Id).FirstOrDefaultAsync();
            if (tmp is not null)
            {
                return new("Stock already exists");
            }
            OrderService.BuyOrders[stock.Id] = new();
            OrderService.SellOrders[stock.Id] = new();
            await dbContext.Stocks.AddAsync(stock);
            await dbContext.SaveChangesAsync();
            return new();
        }

        public async Task<Result> DeleteStockAsync(string id)
        {
            var tmp = await dbContext.Stocks.Where(s => s.Id == id).FirstOrDefaultAsync();
            if (tmp is null)
            {
                return new("Stock doesn't exists");
            }
            if (OrderService.BuyOrders.ContainsKey(id))
            {
                OrderService.BuyOrders.Remove(id);
            }
            if (OrderService.SellOrders.ContainsKey(id))
            {
                OrderService.SellOrders.Remove(id);
            }
            dbContext.Stocks.Remove(tmp);
            var userStock = await dbContext.UserStocks.Where(s => s.StockId == id).ToListAsync();
            dbContext.UserStocks.RemoveRange(userStock);
            await dbContext.SaveChangesAsync();
            return new();
        }
    }
}
