using ESourcing.Sourcing.Entitites;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Data
{
    public class SourcingContextSeed
    {

        public static void SeedData(IMongoCollection<Auction> auctionCollection)
        {
            bool exitAuction = auctionCollection.Find(x => true).Any();

            if (!exitAuction)
            {
                auctionCollection.InsertManyAsync(GetPreconfiguredAuctions());
            }
        }

        private static IEnumerable<Auction> GetPreconfiguredAuctions()
        {
            return new List<Auction>
            {
                new Auction
                {
                    Name="Auction 1",
                    Description="Auction Desc 1",
                    CreatedAt=DateTime.Now,
                    FinishedAt=DateTime.Now,
                    StartedAt=DateTime.Now,
                    ProductId="634d8b05586bff36db298d2e",
                    IncludedSellers=new List<string>()
                    {
                        "seller1@test.com",
                        "seller2@test.com",
                        "seller3@test.com"
                    },
                    Quantity=4,
                    Status=(int)Status.Active
                },
                 new Auction
                {
                    Name="Auction 2",
                    Description="Auction Desc 2",
                    CreatedAt=DateTime.Now,
                    FinishedAt=DateTime.Now,
                    StartedAt=DateTime.Now,
                    ProductId="634d8b05586bff36db298d2e",
                    IncludedSellers=new List<string>()
                    {
                        "seller1@test.com",
                        "seller2@test.com",
                        "seller3@test.com"
                    },
                    Quantity=4,
                    Status=(int)Status.Active
                },
                  new Auction
                {
                    Name="Auction 3",
                    Description="Auction Desc 3",
                    CreatedAt=DateTime.Now,
                    FinishedAt=DateTime.Now,
                    StartedAt=DateTime.Now,
                    ProductId="634d8b05586bff36db298d2e",
                    IncludedSellers=new List<string>()
                    {
                        "seller1@test.com",
                        "seller2@test.com",
                        "seller3@test.com"
                    },
                    Quantity=4,
                    Status=(int)Status.Active
                }
            };
        }
    }
}
