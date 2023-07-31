using ESourcing.Core.Entities;
using ESourcing.Infrastructure.Data;
using ESourcing.Infrastructure.Repositories;
using ESourcing.Infrastructure.Repositories.Base;
using ESourcing.UI.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ESourcing.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebAppContext>(x => x.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            services.AddAutoMapper(typeof(Startup));

            #region IdentityOptions
                services.AddIdentity<AppUser, IdentityRole>(opt =>
                {
                    opt.Password.RequiredLength = 4;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireLowercase = false;
                    opt.Password.RequireUppercase = false;
                    opt.User.RequireUniqueEmail = true;
                    opt.User.AllowedUserNameCharacters = "ABCÇDEFGÐHIÝJKLMNOÖPRSÞTUÜVYZabcçdefgðhýijklmnoöprsþtuüvyz.-_ ";

                }).AddDefaultTokenProviders().AddEntityFrameworkStores<WebAppContext>();
            #endregion

            #region CookieOptions

            services.ConfigureApplicationCookie(x =>
                 {
                     x.LoginPath = "/Home/Login";
                     x.LogoutPath = "/Home/Logout";
                     x.SlidingExpiration = true;
                     x.ExpireTimeSpan = TimeSpan.FromDays(1);
                     x.Cookie.SameSite = SameSiteMode.Strict;
                     x.Cookie.Name = ".Enwy.Security";
                     x.Cookie.HttpOnly = true;

                 });
            #endregion

            services.AddSession(opt=> {

                opt.IdleTimeout = TimeSpan.FromDays(1);
                opt.Cookie.MaxAge = TimeSpan.FromDays(1);
               
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddHttpClient();
            services.AddHttpClient<ProductClient>();
            services.AddHttpClient<AuctionClient>();
            services.AddHttpClient<BidClient>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();



            services.AddAuthentication().AddFacebook(opt =>
            {
                opt.AppId = "825372868696837";
                opt.AppSecret = "af28184fec9e09ceb96fba04c1a70d46";
            }).AddGoogle(opt =>
            {

                opt.ClientId = "845140907201-4oli43tf63thpe8ubp58rott811r65lr.apps.googleusercontent.com";
                opt.ClientSecret = "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";
            });


           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                       name: "Default",
                       pattern: "{controller=Home}/{action=Index}/{id?}"
                 );

            });
        }
    }
}
