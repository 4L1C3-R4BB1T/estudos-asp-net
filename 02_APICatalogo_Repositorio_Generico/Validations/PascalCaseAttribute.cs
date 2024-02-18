using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations;

public class PascalCaseAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }
        var primeiraLetra = value.ToString()[0].ToString();
        if (primeiraLetra != primeiraLetra.ToUpper())
        {
            return new ValidationResult("A primeira letra do nome do produto deve ser maiúscula.");
        }
        return ValidationResult.Success;
    }
}
