using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Extension
{
    public interface IIntegrationEvent : INotification
    {

    }
    public class MyEvent : IIntegrationEvent
    {

    }

    public class IntegrationEventHandler
    : INotificationHandler<IIntegrationEvent>
    {
        public Task Handle(
            IIntegrationEvent integrationEvent,
            CancellationToken cancellationToken)
        {
            //DO SOME WORK
            return Task.CompletedTask;
        }
    }
}
