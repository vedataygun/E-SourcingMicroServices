using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Entitites
{
    public class Bid
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        public string AuctionID { get; set; }

        public string ProductID { get; set; }

        public string Email {get;set;}
        public string SellerUserName { get; set; }
        public decimal price { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
