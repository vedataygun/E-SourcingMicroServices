using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESourcing.Core.Common
{
    public static class RequestUrlAuction
    {
        public static string GetAuctions = "/api/v1/Auction";

        public static string CreateAuctions = "/api/v1/Auction";

        public static string GetAuctionByIdAsync = "/api/v1/Auction/GetAuction/";
    }
}
