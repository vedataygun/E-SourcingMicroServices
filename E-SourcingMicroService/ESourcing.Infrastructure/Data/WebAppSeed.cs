using ESourcing.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESourcing.Infrastructure.Data
{
    public class WebAppSeed
    {
        public static async Task SeedAsync(WebAppContext context,ILoggerFactory loggerFactory , int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                context.Database.Migrate();
                if(!context.AppUsers.Any())
                {
                    context.AppUsers.AddRange(GetPreconfiguredOrders());
                    await context.SaveChangesAsync();

                }

            }
            catch (Exception ex)
            {
                if (retryForAvailability<50)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<WebAppSeed>();
                    log.LogError(ex.Message);
                    Thread.Sleep(2000);
                    await SeedAsync(context,loggerFactory,retry);
                }
            }
        }

        private static IEnumerable<AppUser> GetPreconfiguredOrders()
        {
            return new List<AppUser>()
            {
                new AppUser()
                {
                    FirstName="user_firstname_1",
                    LastName="user_lastname_1",
                    Email="user_1@user.com",
                    IsSeller=true,

                }
            };
        }
    }
}
