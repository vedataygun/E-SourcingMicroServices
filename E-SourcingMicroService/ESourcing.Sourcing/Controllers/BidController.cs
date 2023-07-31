using ESourcing.Sourcing.Data.Interfaces;
using ESourcing.Sourcing.Entitites;
using ESourcing.Sourcing.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        readonly IBidRepository _bidRepository;

        public BidController(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Bid),(int)HttpStatusCode.OK)]
        public async Task<ActionResult> SendBid([FromBody] Bid bid)
        {
            await _bidRepository.SendBid(bid);
            return Ok(bid);
        }
        [HttpGet("GetBidsByAuctionId/{id}")]
        [ProducesResponseType(typeof(Bid), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByAuctionId(string id)
        {
            List<Bid> bids = await _bidRepository.GetBidsByAuctionId(id);
            if(bids==null)
            {
                return NotFound("id bulunamadı");
            }
            return Ok(bids);
        }


        [HttpGet("GetWinnerBid/{id}")]
        [ProducesResponseType(typeof(Bid),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Bid>> GetWinnerBid(string auctionID)
        {
            return Ok(await _bidRepository.GetWinnerBid(auctionID));
        }

    }
}
