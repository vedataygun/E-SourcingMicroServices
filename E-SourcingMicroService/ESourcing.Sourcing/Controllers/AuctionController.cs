using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESourcing.Sourcing.Entitites;
using ESourcing.Sourcing.Repositories.Interfaces;
using System.Net;
using Microsoft.Extensions.Logging;
using EventBusRabbitMQ.Events;
using AutoMapper;
using EventBusRabbitMQ.Producer;
using EventBusRabbitMQ.Core;

namespace ESourcing.Sourcing.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        readonly IAuctionRepository _auctionRepository;
        readonly IBidRepository _bidRepository;
        readonly IMapper _mapper;
        readonly EventBusRabbitMQProducer _eventBus;
        readonly ILogger<AuctionController> _logger;

        public AuctionController(
            EventBusRabbitMQProducer eventBus,
            IMapper mapper,
            IAuctionRepository auctionRepository , 
            IBidRepository bidRepository , 
            ILogger<AuctionController> logger)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _logger = logger;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions() 
        {
            var auctions = await _auctionRepository.GetAuctions();
            return Ok(auctions);
        }

        [HttpGet("{id:length(24)}",Name = "GetAuction")]
        [ProducesResponseType(typeof(Auction),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public async Task<ActionResult<Auction>> GetAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);
            if (auction == null)
            {
                _logger.LogError($"Auction with id: {id} hasn't been found in database");
                return NotFound();
            }

            return Ok(auction);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Auction),(int)HttpStatusCode.Created)]

        public async Task<ActionResult<Auction>> CreateAuction([FromBody]Auction auction)
        {
            await _auctionRepository.Create(auction);

            return CreatedAtRoute("GetAuction", new { id = auction.ID },auction);
        }


        [HttpPut]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> UpdateAuction([FromBody] Auction auction)
        {
            return Ok(await _auctionRepository.Update(auction));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteAuctionById(string id)
        {
            return Ok(await _auctionRepository.Delete(id));
        }


        [HttpPost("CompleteAuction")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]

        public async Task<ActionResult> CompleteAuction([FromBody]string id)
        {
            Auction auction = await _auctionRepository.GetAuction(id);
            if (auction == null)
                return NotFound();

            if (auction.Status!=(int)Status.Active)
            {
                _logger.LogError("Auction can not be completed");
                return BadRequest();
            }

            Bid bid = await _bidRepository.GetWinnerBid(id);
            if (bid == null)
                return NotFound();

            OrderCreateEvent eventMessage = _mapper.Map<OrderCreateEvent>(bid);
            eventMessage.Quantity = auction.Quantity;

            auction.Status = (int)Status.Closed;
            bool updateResult = await _auctionRepository.Update(auction);

            if(!updateResult)
            {
                _logger.LogError("Auction can not updated");
                return BadRequest();
            }

            try
            {
                _eventBus.Publish(EventBusConstans.OrderCreateQueue, eventMessage);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "ERROR publishing integration event : {EventId} from {AppName}", eventMessage.ID, "Sourcing");
                throw;
            }
            return Accepted();
            
        }


        [HttpPost("TestEvent")]
        public ActionResult<OrderCreateEvent> TestEvent()
        {
            OrderCreateEvent orderCreate = new OrderCreateEvent()
            {
                AuctionID = "dummy1",
                ProductID = "dummy_product_1",
                price = 10,
                Quantity = 100,
                SellerUserName = "test@test.com"
            };

            try
            {
                _eventBus.Publish(EventBusConstans.OrderCreateQueue, orderCreate);
            }
            catch(Exception e)
            {
                throw;
            }
            return Accepted(orderCreate);
        }

    }
}
