using System.Globalization;
using PhoneNumbers;

namespace MamisSolidarias.WebAPI.Donors.Extensions;

internal static class StringExtensions
{
    public static string? PrepareForDb(this string? value) 
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();


    public static string? Capitalize(this string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : CultureInfo
                .CurrentCulture
                .TextInfo
                .ToTitleCase(value);

    public static string? ParsePhoneNumber(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        var util = PhoneNumberUtil.GetInstance();
        var phoneNumber = util.Parse(value, "AR");
        return $"+{phoneNumber.CountryCode}{phoneNumber.NationalNumber}";
    }
}