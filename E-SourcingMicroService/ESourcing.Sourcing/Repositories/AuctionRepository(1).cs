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
    public class AuctionRepository : IAuctionRepository
    {

        readonly IMongoCollection<Auction> _AuctionContext;
        readonly IMongoCollection<Bid> _BidContext;

        public AuctionRepository(ISourcingContext context)
        {
            _AuctionContext = context.Auctions;
            _BidContext = context.Bids;
        }

        public async Task Create(Auction auction)
        {
            await _AuctionContext.InsertOneAsync(auction);
        }

        public async Task<bool> Delete(string auctionID)
        {
            var filter = Builders<Auction>.Filter.Eq(z => z.ID, auctionID);
            var DeleteResult = await _AuctionContext.DeleteOneAsync(filter);

            return DeleteResult.IsAcknowledged && DeleteResult.DeletedCount > 0;
        }

        public async Task<Auction> GetAuction(string id)
        {
            return await _AuctionContext.Find(z => z.ID == id).FirstOrDefaultAsync();
        }

        public async Task<Auction> GetAuctionByName(string name)
        {
            var filter = Builders<Auction>.Filter.Eq(x => x.Name, name);
            return await _AuctionContext.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Auction>> GetAuctions()
        {
            return await _AuctionContext.Find(x => true).ToListAsync();

        }

        public async Task<bool> Update(Auction auction)
        {
            var updateResult =await _AuctionContext.ReplaceOneAsync(filter: g => g.ID == auction.ID, replacement: auction);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
