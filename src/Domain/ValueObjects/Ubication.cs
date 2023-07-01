using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public partial record Ubication
{

    private Ubication(string value) => Value = value;
    private const string Pattern = @"";

    public static Ubication? Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        return new Ubication(value);
    }

    public string Value { get; init; }

    [GeneratedRegex(Pattern)]
    private static partial Regex UbicationRegex();
}