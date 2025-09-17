using System.Text.RegularExpressions;

public sealed record Email
{
    private static readonly Regex Pattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Value { get; }

    public Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        Guard.AgainstNullOrEmpty(value, nameof(value));

        if (!Pattern.IsMatch(value))
        {
            throw new ArgumentException("Invalid email format", nameof(value));
        }

        return new(value.Trim().ToLowerInvariant());
    }

    public override string ToString()
    {
        return Value;
    }

    public bool Equals(Email? other) => other is not null && Value == other.Value;


    public static implicit operator string(Email e) => e.Value;
}