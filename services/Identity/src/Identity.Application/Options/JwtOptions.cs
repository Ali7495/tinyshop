public sealed class JwtOptions
{
    public string Key { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public int AccessTokenExpiryMinutes { get; set; } = 15;
} 