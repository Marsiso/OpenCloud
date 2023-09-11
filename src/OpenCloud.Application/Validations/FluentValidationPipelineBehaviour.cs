using FluentValidation;
using MediatR;
using OpenCloud.Domain.Validators;

namespace Cloud.Application.Validations;

public class FluentValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public FluentValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (!_validators.Any()) return await next();

		var validationContext = new ValidationContext<TRequest>(request);

		var validationFailures = _validators.Select(validator => validator.Validate(validationContext));

		if (validationFailures is null) return await next();

		var validationErrors = validationFailures.DistinctErrorsByProperty();

		if (validationErrors.Any()) throw new FluentValidationEntityException(typeof(TRequest).Name, validationErrors);

		return await next();
	}
}