namespace MatchingEngine.Hubs
{
    public class OrderHub : Hub
    {
        private readonly AppDbContext dbContext;
        private readonly TransactionService transactionService;
        public OrderHub(AppDbContext dbContext, TransactionService transactionService)
        {
            this.dbContext = dbContext;
            this.transactionService = transactionService;
        }


        async Task<Result> CheckOrder(Order order)
        {
            if (order is null || order.Quantity == 0)
            {
                return new("Invalid order");
            }
            var stock = await dbContext.Stocks.Where(s => s.Id == order.StockId).FirstOrDefaultAsync();
            if (stock is null)
            {
                return new("Stock doesn't exist");
            }
            if (order.OrderType == OrderType.BUY)
            {
                var cost = order.Price * order.Quantity;
                var user = await dbContext.Users.Where(s => s.Id == order.CustomerId).FirstOrDefaultAsync();
                if (user is null)
                {
                    return new("User doesn't exist");
                }
                if (user.Balance < cost)
                {
                    return new("User's balance is not enough");
                }
                if (!OrderService.BuyOrders.ContainsKey(stock.Id))
                {
                    OrderService.BuyOrders[stock.Id] = new();
                }
                if (!OrderService.SellOrders.ContainsKey(stock.Id))
                {
                    OrderService.SellOrders[stock.Id] = new();
                }
                return new();
            }
            else
            {
                var user = await dbContext.Users.Where(s => s.Id == order.CustomerId).FirstOrDefaultAsync();
                if (user is null)
                {
                    return new("User doesn't exist");
                }
                var userStock = await dbContext.UserStocks.Where(s => s.Id == user.Id + order.StockId).FirstOrDefaultAsync();
                if (userStock is null || userStock.Quantity < order.Quantity)
                {
                    return new("Invalid quantity");
                }
                if (!OrderService.BuyOrders.ContainsKey(stock.Id))
                {
                    OrderService.BuyOrders[stock.Id] = new();
                }
                if (!OrderService.SellOrders.ContainsKey(stock.Id))
                {
                    OrderService.SellOrders[stock.Id] = new();
                }
                return new();
            }
        }

        public async Task SendSellOrder(Order order)
        {
            var checkResult = await CheckOrder(order);
            if (checkResult.Success)
            {
                var maxBuyOrder = OrderService.BuyOrders[order.Id].Max;
                while (!(await CheckOrder(maxBuyOrder)).Success)
                {
                    OrderService.BuyOrders[order.Id].Remove(maxBuyOrder);
                    await Clients.All.SendAsync("DeleteBuyOrder", maxBuyOrder);
                    maxBuyOrder = OrderService.BuyOrders[order.Id].Max;
                    if (maxBuyOrder is null)
                    {
                        break;
                    }
                }
                while (maxBuyOrder is not null && maxBuyOrder.Price >= order.Price)
                {
                    var q = Math.Min(order.Quantity, maxBuyOrder.Quantity);
                    Transaction transaction = new()
                    {
                        BuyerId = maxBuyOrder.CustomerId,
                        SellerId = order.Id,
                        Quantity = q,
                        Price = order.Price
                    };
                    await transactionService.AddTransactionAsync(transaction);
                    await Clients.Client(Context.ConnectionId).SendAsync("ExecuteTransaction", transaction);
                    order.Quantity -= q;
                    maxBuyOrder.Quantity -= q;
                    if (order.Quantity == 0)
                    {
                        break;
                    }
                    else
                    {
                        OrderService.BuyOrders[order.Id].Remove(maxBuyOrder);
                        await Clients.All.SendAsync("DeleteBuyOrder", maxBuyOrder);
                        maxBuyOrder = OrderService.BuyOrders[order.Id].Max;
                        while (!(await CheckOrder(maxBuyOrder)).Success)
                        {
                            OrderService.BuyOrders[order.Id].Remove(maxBuyOrder);
                            await Clients.All.SendAsync("DeleteBuyOrder", maxBuyOrder);
                            maxBuyOrder = OrderService.BuyOrders[order.Id].Max;
                            if (maxBuyOrder is null)
                            {
                                break;
                            }
                        }
                    }
                }
                if (order.Quantity > 0)
                {
                    OrderService.SellOrders[order.Id].Add(order);
                    await Clients.All.SendAsync("ReceiveSellOrder", order);
                }
            }
        }

        public async Task SendBuyOrder(Order order)
        {
            var checkResult = await CheckOrder(order);
            if (checkResult.Success)
            {
                var minSellOrder = OrderService.SellOrders[order.Id].Min;
                while (!(await CheckOrder(minSellOrder)).Success)
                {
                    OrderService.SellOrders[order.Id].Remove(minSellOrder);
                    await Clients.All.SendAsync("DeleteSellOrder", minSellOrder);
                    minSellOrder = OrderService.SellOrders[order.Id].Max;
                    if (minSellOrder is null)
                    {
                        break;
                    }
                }
                while (minSellOrder is not null && minSellOrder.Price <= order.Price)
                {
                    var q = Math.Min(order.Quantity, minSellOrder.Quantity);
                    Transaction transaction = new()
                    {
                        BuyerId = minSellOrder.CustomerId,
                        SellerId = order.Id,
                        Quantity = q,
                        Price = order.Price
                    };
                    await transactionService.AddTransactionAsync(transaction);
                    await Clients.Client(Context.ConnectionId).SendAsync("ExecuteTransaction", transaction);
                    order.Quantity -= q;
                    minSellOrder.Quantity -= q;
                    if (order.Quantity == 0)
                    {
                        break;
                    }
                    else
                    {
                        OrderService.SellOrders[order.Id].Remove(minSellOrder);
                        await Clients.All.SendAsync("DeleteSellOrder", minSellOrder);
                        minSellOrder = OrderService.SellOrders[order.Id].Max;
                        while (!(await CheckOrder(minSellOrder)).Success)
                        {
                            OrderService.SellOrders[order.Id].Remove(minSellOrder);
                            await Clients.All.SendAsync("DeleteSellOrder", minSellOrder);
                            minSellOrder = OrderService.SellOrders[order.Id].Max;
                            if (minSellOrder is null)
                            {
                                break;
                            }
                        }
                    }
                }
                if (order.Quantity > 0)
                {
                    OrderService.SellOrders[order.Id].Add(order);
                    await Clients.All.SendAsync("ReceiveBuyOrder", order);
                }
            }
        }

        public async Task GetOrderBook()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("SendOrderBook", (OrderService.BuyOrders, OrderService.SellOrders));
        }
    }
}
