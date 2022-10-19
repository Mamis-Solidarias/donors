using System.Globalization;

namespace MamisSolidarias.WebAPI.Donors.Extensions;

internal static class StringExtensions
{
    public static string? PrepareForDb(this string? value) 
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();


    public static string Capitalize(this string value) =>
        CultureInfo
            .CurrentCulture
            .TextInfo
            .ToTitleCase(value);
}