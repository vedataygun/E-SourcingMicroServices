using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Commands.OrderCreate
{
    public class OrderCreateValidater : AbstractValidator<OrderCreateCommand>
    {
        public OrderCreateValidater()
        {
            RuleFor(v => v.ProductId).NotEmpty().WithMessage("ProductId boş bırakılamaz.");
        }
    }
}
