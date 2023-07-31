using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext):base(orderContext)
        {
           
        }
        public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(string id)
        {
            return await _context.Orders.Where(o => o.SellerUserName == id).ToListAsync();
        }
    }
}
