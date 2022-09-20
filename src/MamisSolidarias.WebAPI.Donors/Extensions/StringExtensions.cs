using System.Globalization;

namespace MamisSolidarias.WebAPI.Donors.Extensions;

internal static class StringExtensions
{
    public static string PrepareForDb(this string value) => 
        value.Trim().ToLowerInvariant();

    public static string Capitalize(this string value) =>
        CultureInfo
            .CurrentCulture
            .TextInfo
            .ToTitleCase(value);
}