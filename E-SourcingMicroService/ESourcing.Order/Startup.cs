using AutoMapper;
using ESourcing.Order.Consumer;
using ESourcing.Order.Extensions;
using EventBusRabbitMQ;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ordering.Application;
using Ordering.Application.Commands.OrderCreate;
using Ordering.Infrastructure;
using RabbitMQ.Client;
using System.Reflection;

namespace ESourcing.Order
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
            services.AddControllers();


            #region AddServiceCollection
            services.AddInfrastructure(Configuration);
            services.AddApplication();
            services.AddAutoMapper(typeof(Startup));
          

            #endregion

            #region Swagger Dependencies
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "ESourcing.Orders",
                    Version = "v1"
                });
            });
            #endregion

            #region EventBusRabbitMQ
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
    {
        var logger = sp.GetRequiredService<ILogger<DefaultMQPersistentConnection>>();

        var factory = new ConnectionFactory()
        {
            HostName = Configuration["EventBus:HostName"]
        };

        if (!string.IsNullOrWhiteSpace(Configuration["EventBus:UserName"]))
        {
            factory.UserName = Configuration["EventBus:UserName"];
        }

        if (!string.IsNullOrWhiteSpace(Configuration["EventBus:Password"]))
        {
            factory.UserName = Configuration["EventBus:Password"];
        }


        int retryCount = 5;

        if (!string.IsNullOrWhiteSpace(Configuration["EventBus:RetryCount"]))
        {
            retryCount = int.Parse(Configuration["EventBus:RetryCount"]);
        }

        return new DefaultMQPersistentConnection(factory, retryCount, logger);

        });

            services.AddSingleton<EventBusOrderCreateConsumer>();
   



            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Api v1"));
            }


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRabbitListener();
        }
    }
}
