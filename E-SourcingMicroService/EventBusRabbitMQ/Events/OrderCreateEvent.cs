using EventBusRabbitMQ.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Events
{
    public class OrderCreateEvent : IEvent
    {

        public string ID { get; set; }

        public string Email { get; set; }
        public string AuctionID { get; set; }

        public string ProductID { get; set; }

        public string SellerUserName { get; set; }

        public decimal price { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
