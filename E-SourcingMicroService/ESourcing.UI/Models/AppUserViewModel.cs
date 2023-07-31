using AutoMapper;
using ESourcing.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace ESourcing.UI.Models
{
    public class AppUserViewModel
    {

        [Required(ErrorMessage ="UserName is Required")]
        [Display(Name ="Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "FirstName is Required")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is Required")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is Required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Is Buyer is Required")]
        [Display(Name = "Is Buyer")]
        public bool IsBuyer{ get; set; }

        [Required(ErrorMessage = "Is Seller is Required")]
        [Display(Name = "Is Seller")]
        public bool IsSeller { get; set; }

        public int UserSelectTypeId { get; set;}


        public AppUser getUser(IMapper mapper)
        {
            if (this.UserSelectTypeId == 1)
                this.IsBuyer = true;
            else if (this.UserSelectTypeId == 2)
                this.IsSeller = true;

            return mapper.Map<AppUser>(this);
        }
    }
}
