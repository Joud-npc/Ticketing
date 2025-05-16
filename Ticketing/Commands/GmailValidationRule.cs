namespace Ticketing.Commands;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

public class HogwartsEmailValidationRule : ValidationRule
{
    public static readonly List<string> HogwartsHouses = new List<string>
    {
        "gryffondor.hp",
        "poufsouffle.hp",
        "serdaigle.hp",
        "serpentard.hp"
    };

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        string email = value as string ?? string.Empty;

        bool isValid = HogwartsHouses.Any(house => email.EndsWith("@" + house, StringComparison.OrdinalIgnoreCase));

        if (!isValid)
        {
            return new ValidationResult(false, 
                "L'email doit se terminer par @gryffondor.hp, @poufsouffle.hp, @serdaigle.hp ou @serpentard.hp");
        }

        return ValidationResult.ValidResult;
    }
}