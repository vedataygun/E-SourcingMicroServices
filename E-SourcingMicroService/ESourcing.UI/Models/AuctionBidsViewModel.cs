using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.UI.Models
{
    public class AuctionBidsViewModel
    {

        public string ID { get; set; }
        public string ProductId { get; set; }
        public string Email { get; set; }
        public bool isAdmin { get; set; }
        public string SellerUserName { get; set; }
        public List<BidViewModel> Bids { get; set; }
    }
}
