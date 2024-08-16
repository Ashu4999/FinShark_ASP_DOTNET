using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class AllowedValuesAttribute : ValidationAttribute
{
    private readonly string[] _allowedValues;

    public AllowedValuesAttribute(params string[] allowedValues)
    {
        _allowedValues = allowedValues;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || value is not string stringValue)
        {
            return ValidationResult.Success; // Null is allowed due to the nullable string
        }

        if (_allowedValues.Contains(stringValue))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult($"The value must be one of the following: {string.Join(", ", _allowedValues)}.");
    }
}
