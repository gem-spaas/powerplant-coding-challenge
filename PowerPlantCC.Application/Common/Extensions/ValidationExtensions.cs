using FluentValidation;
using FluentValidation.Validators;

namespace PowerPlantCC.Application.Common.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> GreaterThanOrEqualToWithMessage<T, TProperty>(
        this IRuleBuilder<T, TProperty> ruleBuilder,
        TProperty valueToCompare,
        string errorMessage = "{PropertyName} must be greater than or equal to {ComparisonValue}."
    ) where TProperty : IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .SetValidator(new GreaterThanOrEqualValidator<T, TProperty>(valueToCompare))
            .WithMessage(errorMessage);
    }
}