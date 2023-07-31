using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Core;
using EventBusRabbitMQ.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ordering.Application.Commands.OrderCreate;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESourcing.Order.Consumer
{
    public class EventBusOrderCreateConsumer
    {
        readonly IRabbitMQPersistentConnection _persistentConnection;
        readonly IMapper _mapper;
        private IServiceScopeFactory _serviceScopeFactory;

        public EventBusOrderCreateConsumer(IRabbitMQPersistentConnection persistentConnection, IMapper mapper  , IServiceScopeFactory serviceScopeFactory)
        {
            _persistentConnection = persistentConnection;
            _mapper = mapper;
            _serviceScopeFactory = serviceScopeFactory;
        }



        public void Consume()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();

            }

            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstans.OrderCreateQueue, durable: false, exclusive: false,autoDelete:false, arguments: null);

            var consomer = new EventingBasicConsumer(channel);

            consomer.Received +=ReceivedEvent;

            channel.BasicConsume(queue: EventBusConstans.OrderCreateQueue, autoAck: true, consumer: consomer);
        }

        public async void ReceivedEvent(object sender , BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            var @event = JsonConvert.DeserializeObject<OrderCreateEvent>(message);

            if(e.RoutingKey== EventBusConstans.OrderCreateQueue)
            {
                var command = _mapper.Map<OrderCreateCommand>(@event);

                command.CreatedAt = DateTime.Now;
                command.TotalPrice = @event.Quantity * @event.price;
                command.UnitPrice = @event.price;

                  using(var scope = _serviceScopeFactory.CreateScope())
                 {
                  var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(command);
                 }
               
            }
        }

  
        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }
    }
}
