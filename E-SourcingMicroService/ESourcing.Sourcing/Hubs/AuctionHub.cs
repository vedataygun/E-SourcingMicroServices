using ESourcing.Sourcing.Repositories;
using ESourcing.Sourcing.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Hubs
{
    public class AuctionHub : Hub
    {
        readonly IBidRepository _bidRepository;

        public AuctionHub(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task AddToGroupAsync(string userId,string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);
        }

        public async Task SendBidAsync(string groupName,string auctionId)
        {
            var bids = await _bidRepository.GetBidsByAuctionId(auctionId);
            await Clients.Group(groupName).SendAsync("Bids",bids);
        }

        public async Task SendCompleteBidAsync(string groupName , string CompletedAuctionId)
        {
            await Clients.Group(groupName).SendAsync("CompleteBid", CompletedAuctionId);
        }

        public class CustomNameProvider : IUserIdProvider
        {
            public virtual string GetUserId(HubConnectionContext connection)
            {
                return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
            }
        }
    }


}
