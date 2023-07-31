using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ordering.Domain.Entities;
using System.Net;
using Ordering.Application.Response;
using Ordering.Application.Queries;
using Ordering.Application.Commands.OrderCreate;

namespace ESourcing.Order.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet("GetOrdersByUserName/{username}")]
        [ProducesResponseType(typeof(OrderResponse),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string username)
        {
            var query = new GetOrdersBySellerUsernameQuery(username);
            var orders = await _mediator.Send(query);

            if (orders.Count() == decimal.Zero)
                return NotFound();

            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderResponse),(int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(OrderResponse),(int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<OrderResponse>> OrderCreate([FromBody] OrderCreateCommand command)
        {
            var result = await _mediator.Send(command);
        
            //if(result.ErrorMessageList != null)
            //{
            //    string errorMessage = "";
            //    foreach(var i  in result.ErrorMessageList)
            //    {
            //        errorMessage += i + " \n";
            //    }
            //    return NotFound(errorMessage);
            //}
                
             return Ok(result);
        }

    }
}
