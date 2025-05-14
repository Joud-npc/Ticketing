namespace Ticketing.Commands;

using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

public class GmailValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        string email = value as string ?? string.Empty;

        string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";

        if (!Regex.IsMatch(email, pattern))
        {
            return new ValidationResult(false, "L'email doit se terminer par @gmail.com.");
        }

        return ValidationResult.ValidResult;
    }
}
