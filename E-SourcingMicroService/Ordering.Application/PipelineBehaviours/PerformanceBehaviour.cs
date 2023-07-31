using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.PipelineBehaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)         
        {
            _timer.Start();
            var response = await next();

            _timer.Stop();
            var elapsedMiliSeconds = _timer.ElapsedMilliseconds;
            if (elapsedMiliSeconds > 600)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogWarning("Long running Requst {requestName} ,{elapsedMiliSeconds} miliseconds", requestName, elapsedMiliSeconds);
            }
            return response;
        }
    }
}
