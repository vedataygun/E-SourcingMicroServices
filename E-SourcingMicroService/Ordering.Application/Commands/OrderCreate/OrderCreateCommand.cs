﻿using MediatR;
using Ordering.Application.Response;
using System;

namespace Ordering.Application.Commands.OrderCreate
{
    public class OrderCreateCommand : IRequest<OrderResponse>
    {
        public string AuctionId { get; set; }
        public string SellerUserName { get; set; }
        public string Email { get; set; }
        public string ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
