using ESourcing.Sourcing.Data.Interfaces;
using ESourcing.Sourcing.Entitites;
using ESourcing.Sourcing.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Repositories
{
    public class BidRepository : IBidRepository
    {
        readonly IMongoCollection<Bid> _bidContext;

        public BidRepository(ISourcingContext context)
        {
            _bidContext = context.Bids;
        }

        public async Task<List<Bid>> GetBidsByAuctionId(string id)
        {
            var filter = Builders<Bid>.Filter.Eq(x => x.AuctionID, id);
            List<Bid> bids = await _bidContext.Find(filter).ToListAsync();

            bids = bids.OrderByDescending(x => x.CreatedAt).GroupBy(x => x.SellerUserName)
                .Select(a => new Bid
                {
                    AuctionID = a.FirstOrDefault().AuctionID,
                    CreatedAt=a.FirstOrDefault().CreatedAt,
                    ID=a.FirstOrDefault().ID,
                    SellerUserName=a.FirstOrDefault().SellerUserName,
                    price=a.FirstOrDefault().price,
                    ProductID=a.FirstOrDefault().ProductID,
                    Email=a.FirstOrDefault().Email
                }).ToList();

            return bids;

        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            List<Bid> bids = await GetBidsByAuctionId(id);
            return bids.OrderByDescending(x => x.price).FirstOrDefault();
        }

        public async Task SendBid(Bid bid)
        {
            await _bidContext.InsertOneAsync(bid);
        }
    }
}
