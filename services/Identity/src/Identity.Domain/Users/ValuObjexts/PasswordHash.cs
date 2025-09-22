public sealed record PasswordHash
{
    public string Value { get; set; }

    public PasswordHash(string value) => Value = value;

    public static PasswordHash FromHashed(string hash)
    {
        Guard.AgainstNullOrEmpty(hash, nameof(hash));

        return new(hash);
    }

    public override string ToString() => Value;

    public bool Equals(PasswordHash? other) => other is not null && Value == other.Value;
}