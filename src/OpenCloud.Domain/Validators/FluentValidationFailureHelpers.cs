using FluentValidation.Results;

namespace OpenCloud.Domain.Validators;

public static class FluentValidationFailureHelpers
{
	public static Dictionary<string, string[]> DistinctErrorsByProperty(this ValidationResult? validationResult)
	{
		if (validationResult is null) return new Dictionary<string, string[]>();

		return validationResult.Errors
			.GroupBy(
				vf => vf.PropertyName,
				vf => vf.ErrorMessage,
				(pn, em) => new
				{
					Key = pn,
					Values = em.Distinct().ToArray()
				})
			.ToDictionary(
				pn => pn.Key,
				vf => vf.Values);
	}

	public static Dictionary<string, string[]> DistinctErrorsByProperty(this IEnumerable<ValidationResult>? validationResults)
	{
		if (validationResults is null) return new Dictionary<string, string[]>();

		return validationResults
			.Where(vr => vr is { IsValid: false, Errors: not null, Errors.Count: > 0 })
			.SelectMany(vr => vr.Errors, (_, vf) => vf)
			.GroupBy(
				vf => vf.PropertyName,
				vf => vf.ErrorMessage,
				(pn, em) => new
				{
					Key = pn,
					Values = em.Distinct().ToArray()
				})
			.ToDictionary(
				pn => pn.Key,
				vf => vf.Values);
	}
}