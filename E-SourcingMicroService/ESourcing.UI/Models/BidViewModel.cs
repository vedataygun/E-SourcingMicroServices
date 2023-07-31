﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.UI.Models
{
    public class BidViewModel
    {
        public string Id { get; set; }
        public string AuctionId { get; set; }
        public string ProductId { get; set; }

        public string Email { get; set; }
        public string SellerUserName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
