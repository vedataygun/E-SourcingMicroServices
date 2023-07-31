using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetpreconfiguredOrder());
                await orderContext.SaveChangesAsync();
            }
        }

        private static List<Order> GetpreconfiguredOrder()
        {
            return new List<Order>()
            {
                new Order
                {
                    AuctionId=Guid.NewGuid().ToString(),
                    ProductId=Guid.NewGuid().ToString(),
                    SellerUserName="test@test.com",
                    UnitPrice=12,
                    TotalPrice=1700,
                    CreatedAt=DateTime.UtcNow

                },
                 new Order
                {
                    AuctionId=Guid.NewGuid().ToString(),
                    ProductId=Guid.NewGuid().ToString(),
                    SellerUserName="test1@test.com",
                    UnitPrice=19,
                    TotalPrice=1500,
                    CreatedAt=DateTime.UtcNow

                },
                 new Order
                {
                    AuctionId=Guid.NewGuid().ToString(),
                    ProductId=Guid.NewGuid().ToString(),
                    SellerUserName="test2@test.com",
                    UnitPrice=15,
                    TotalPrice=1200,
                    CreatedAt=DateTime.UtcNow

                }

            };
        }
    }
}
