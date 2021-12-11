namespace MatchingEngine
{
    public class MatchingAlgorithms
    {
        public static OrderExecutionResult MatchingAlgorithm1(List<Order> BuyOrders, List<Order> SellOrders, List<Transaction> Transactions, Dictionary<string, double> BalanceChanges, string StockId, bool Sorted = false)
        {
            if (!Sorted)
            {
                BuyOrders.Sort();
                SellOrders.Sort();
            }
            var SellOrderLeft = new List<Order>();
            var j = SellOrders.Count - 1;
            var i = BuyOrders.Count - 1;
            while (i >= 0 && j >= 0)
            {
                while (j >= 0 && SellOrders[j].Price > BuyOrders[i].Price)
                {
                    SellOrderLeft.Add(SellOrders[j]);
                    j--;
                }
                if (j < 0)
                {
                    break;
                }
                else
                {
                    while (BuyOrders[i].Quantity > 0 && j >= 0)
                    {
                        var q = (BuyOrders[i].Quantity <= SellOrders[j].Quantity) ? BuyOrders[i].Quantity : SellOrders[j].Quantity;
                        var p = SellOrders[j].Price;
                        BuyOrders[i].Quantity -= q;
                        SellOrders[j].Quantity -= q;
                        Transactions.Add(new()
                        {
                            BuyerId = BuyOrders[i].CustomerId,
                            SellerId = SellOrders[j].CustomerId,
                            Price = p,
                            Quantity = q,
                            StockId = StockId
                        });
                        if (!BalanceChanges.ContainsKey(BuyOrders[i].CustomerId))
                        {
                            BalanceChanges[BuyOrders[i].CustomerId] = 0;
                        }
                        if (!BalanceChanges.ContainsKey(SellOrders[j].CustomerId))
                        {
                            BalanceChanges[SellOrders[j].CustomerId] = 0;
                        }
                        var c = p * q;
                        BalanceChanges[BuyOrders[i].CustomerId] -= c;
                        BalanceChanges[SellOrders[j].CustomerId] += c;
                        if (SellOrders[j].Quantity == 0)
                        {
                            j--;
                        }
                    }
                    BuyOrders.RemoveAt(i);
                    i--;
                }
            }
            //Parallel.ForEach(BalanceChanges.Keys, k =>
            //{
            //    BalanceChanges[k] = Math.Round(BalanceChanges[k], 2);
            //});
            foreach (var k in BalanceChanges.Keys)
            {
                BalanceChanges[k] = Math.Round(BalanceChanges[k], 2);
            }
            return new()
            {
                BuyOrdersLeft = BuyOrders,
                SellOrdersLeft = SellOrderLeft
            };
        }


        public static OrderExecutionResult MatchingAlgorithm2(List<Order> BuyOrders, List<Order> SellOrders, List<Transaction> Transactions, Dictionary<string, double> BalanceChanges, string StockId, bool Sorted = false)
        {
            if (!Sorted)
            {
                BuyOrders.Sort();
                SellOrders.Sort();
            }
            var i = BuyOrders.Count - 1;
            var j = SellOrders.Count - 1;
            var Found = true;
            while (Found && i >= 0 && j >= 0)
            {
                if (SellOrders[j].Price > BuyOrders[i].Price)
                {
                    Found = false;
                    var l = 0;
                    var r = j - 1;
                    while (l <= r)
                    {
                        var mid = l + ((r - l) >> 1);
                        if (SellOrders[mid].Price <= BuyOrders[i].Price && SellOrders[mid + 1].Price > BuyOrders[i].Price)
                        {
                            Found = true;
                            j = mid;
                            break;
                        }
                        else if (SellOrders[mid].Price <= BuyOrders[i].Price)
                        {
                            l = mid + 1;
                        }
                        else
                        {
                            r = mid - 1;
                        }
                    }
                    if (!Found)
                    {
                        j = -1;
                    }
                }
                if (j == -1)
                {
                    break;
                }
                else
                {
                    while (BuyOrders[i].Quantity > 0 && j >= 0)
                    {
                        var q = (BuyOrders[i].Quantity <= SellOrders[j].Quantity) ? BuyOrders[i].Quantity : SellOrders[j].Quantity;
                        var p = SellOrders[j].Price;
                        BuyOrders[i].Quantity -= q;
                        SellOrders[j].Quantity -= q;
                        Transactions.Add(new()
                        {
                            BuyerId = BuyOrders[i].CustomerId,
                            SellerId = SellOrders[j].CustomerId,
                            Price = p,
                            Quantity = q,
                            StockId = StockId
                        });
                        if (!BalanceChanges.ContainsKey(BuyOrders[i].CustomerId))
                        {
                            BalanceChanges[BuyOrders[i].CustomerId] = 0;
                        }
                        if (!BalanceChanges.ContainsKey(SellOrders[j].CustomerId))
                        {
                            BalanceChanges[SellOrders[j].CustomerId] = 0;
                        }
                        var c = p * q;
                        BalanceChanges[BuyOrders[i].CustomerId] -= c;
                        BalanceChanges[SellOrders[j].CustomerId] += c;
                        if (SellOrders[j].Quantity == 0)
                        {
                            j--;
                        }
                    }
                    i--;
                }
            }
            var BuyOrderLeft = new List<Order>();
            var SellOrderLeft = new List<Order>();
            for (int k = 0; k < i + 1; k++)
            {
                BuyOrderLeft.Add(BuyOrders[k]);
            }
            foreach (var order in SellOrders)
            {
                if (order.Quantity > 0)
                {
                    SellOrderLeft.Add(order);
                }
            }
            //Parallel.ForEach(BalanceChanges.Keys, k =>
            //{
            //    BalanceChanges[k] = Math.Round(BalanceChanges[k], 2);
            //});
            foreach (var k in BalanceChanges.Keys)
            {
                BalanceChanges[k] = Math.Round(BalanceChanges[k], 2);
            }
            return new()
            {
                BuyOrdersLeft = BuyOrderLeft,
                SellOrdersLeft = SellOrderLeft
            };
        }


        public static Dictionary<string, SeperatedOrders> SeperateSortedOrders(List<Order> BuyOrders, List<Order> SellOrders)
        {
            BuyOrders.Sort();
            SellOrders.Sort();
            var res = new Dictionary<string, SeperatedOrders>();
            for (int i = 0; i < BuyOrders.Count; i++)
            {
                var order = BuyOrders[i];
                if (!res.ContainsKey(order.StockId))
                {
                    res[order.StockId] = new() { StockId = order.StockId };
                }
                res[order.StockId].BuyOrders.Add(order);
            }
            for (int i = 0; i < SellOrders.Count; i++)
            {
                var order = SellOrders[i];
                if (!res.ContainsKey(order.StockId))
                {
                    res[order.StockId] = new() { StockId = order.StockId };
                }
                res[order.StockId].SellOrders.Add(order);
            }
            return res;
        }


        public static OrderExecutionResult ExecuteOrders(List<Order> BuyOrders, List<Order> SellOrders)
        {
            var SeperatedRes = SeperateSortedOrders(BuyOrders, SellOrders);
            List<Transaction> Transactions = new();
            Dictionary<string, double> BalanceChanges = new();
            List<Order> BuyOrdersLeft = new();
            List<Order> SellOrdersLeft = new();
            foreach (var stockId in SeperatedRes.Keys)
            {
                var tmp = MatchingAlgorithm1(SeperatedRes[stockId].BuyOrders, SeperatedRes[stockId].SellOrders, Transactions, BalanceChanges, stockId, true);
                foreach (var order in tmp.BuyOrdersLeft)
                {
                    BuyOrdersLeft.Add(order);
                }
                foreach (var order in tmp.SellOrdersLeft)
                {
                    SellOrdersLeft.Add(order);
                }
            }
            return new()
            {
                BalanceChanges = BalanceChanges,
                Transactions = Transactions,
                BuyOrdersLeft = BuyOrdersLeft,
                SellOrdersLeft = SellOrdersLeft
            };
        }
    }
}
