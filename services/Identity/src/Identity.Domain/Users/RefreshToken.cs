public sealed class RefreshToken : Entity
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool Revoked { get; set; }
    public DateTime CreatedAt { get; set; }

    public RefreshToken()
    {

    }

    public RefreshToken(Guid id, Guid userId, string token, DateTime expiresAt) : base(id)
    {
        Guard.AgainstNullOrEmpty(token, nameof(token));
        Guard.Against(expiresAt <= DateTime.Now, "Refresh token must expires in the future!");

        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.Now;
    }

    public static RefreshToken Issue(Guid userId, string token, DateTime expiresAt)
    => new(Guid.NewGuid(), userId, token, expiresAt);

    public bool IsActive => !Revoked && ExpiresAt < DateTime.Now;

    public void Revoke()
    {
        if (Revoked)
        {
            return;
        }

        Revoked = true;
    }
}