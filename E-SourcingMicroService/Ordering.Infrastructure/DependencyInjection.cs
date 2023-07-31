using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Repositories;
using Ordering.Domain.Repositories.Base;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;
using Ordering.Infrastructure.Repositories.Base;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<OrderContext>(opt => opt.UseInMemory(database:"InMemory"),
            //                                                                    ServiceLifetime.Singleton,
            //                                                                    ServiceLifetime.Singleton);


            services.AddDbContext<OrderContext>(opt => opt.UseSqlServer
                (configuration.GetConnectionString("OrderDatabaseConnection"),
                    b => b.MigrationsAssembly(typeof(OrderContext).Assembly.FullName))
                ,ServiceLifetime.Transient
                        );


            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
