using System.Net.Http.Headers;

public sealed class User : AggregateRoot
{
    private readonly HashSet<string> _roles = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<RefreshToken> _refreshTokens = new();



    public Email Email { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null;
    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<string> Roles => _roles;
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

    private User() { }

    private User(Guid id, Email email, PasswordHash passwordHash) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.Now;
    }

    public static User Register(Email email, PasswordHash passwordHash, IEnumerable<string>? initialRoles)
    {
        User user = new(Guid.NewGuid(), email, passwordHash);

        if (initialRoles is not null)
        {
            foreach (string r in initialRoles)
            {
                user.AddRole(r);
            }
        }

        user.Raise(new UserRegisteredEvent(user.Id, user.Email));

        return user;
    }

    public void ChangePassword(PasswordHash newHashedPassword)
    {
        PasswordHash = newHashedPassword;
    }


    public void AddRole(string role)
    {
        Guard.AgainstNullOrEmpty(role, nameof(role));

        _roles.Add(role.Trim());
    }

    public void RemoveRole(string role)
    {
        Guard.AgainstNullOrEmpty(role, nameof(role));

        _roles.Remove(role.Trim());
    }

    public RefreshToken AddRefreshToken(string token, DateTime expiresAt)
    {
        RefreshToken refreshToken = RefreshToken.Issue(Id, token, expiresAt);
        _refreshTokens.Add(refreshToken);

        return refreshToken;
    }

    public void RevokeRefreshToken(string token)
    {
        RefreshToken existing = _refreshTokens.FirstOrDefault(r => r.Token == token);
        if (existing is null)
            return;
        _refreshTokens.Remove(existing);    
    }
}