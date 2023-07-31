using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ESourcing.UI.Models
{
    public class AuctionViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please fill name")]

        public string Name { get; set; }

        [Required(ErrorMessage = "Please fill Description")]

        public string Description { get; set; }

        [Required(ErrorMessage = "Please fill ProductId")]

        public string ProductId{ get; set; }
        [Required(ErrorMessage = "Please fill Quantity")]

        public int Quantity{ get; set; }
        public DateTime StartedAt{ get; set; }

        [Required(ErrorMessage = "Please fill FinishedAt")]

        public DateTime FinishedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public int Status { get; set; }

        public int SellerId { get; set; }

        public List<String> IncludedSellers { get; set; }
    }
}
