using FluentValidation;
using MediatR;

namespace Application.Mediator.Behavior
{
    public class ValidationBehavior<TRequset, TResponse> : IPipelineBehavior<TRequset, TResponse>
        where TRequset : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequset>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequset>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequset request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            var context = new ValidationContext<TRequset>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            var response = await next();


            return response;
        }
    }
}

