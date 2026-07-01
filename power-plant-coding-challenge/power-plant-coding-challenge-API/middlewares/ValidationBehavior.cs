using FluentValidation;
using MediatR;

namespace power_plant_coding_challenge_API.Middlewares;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var failures = validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(error => error is not null)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next(cancellationToken);
    }
}
