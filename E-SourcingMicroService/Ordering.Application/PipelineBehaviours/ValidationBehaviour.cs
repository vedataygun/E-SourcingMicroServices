using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Ordering.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.PipelineBehaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest:IRequest<TResponse>
    {
        readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }


        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            var context = new ValidationContext<TRequest>(request);
            var fail = _validators.Select(x => x.Validate(context))
                       .SelectMany(x => x.Errors).Where(x => x != null).ToList();

            if (fail.Any())
            {
                throw new ValidationException(fail.ToString());
            }
            return next();
        }
    }
}
