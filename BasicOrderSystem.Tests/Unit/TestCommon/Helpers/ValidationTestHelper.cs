using FluentValidation;
using FluentValidation.TestHelper;

namespace BasicOrderSystem.Tests.Unit.TestCommon.Helpers;

public static class ValidationTestHelper
{
    public static void ShouldHaveValidationError<T>(this IValidator<T> validator, T model, string propertyName)
    {
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(propertyName);
    }

    public static void ShouldNotHaveValidationError<T>(this IValidator<T> validator, T model, string propertyName)
    {
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(propertyName);
    }
}