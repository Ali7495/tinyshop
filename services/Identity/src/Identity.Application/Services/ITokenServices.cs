using System.Security.Claims;

public interface ITokenServices
{
    string GenerateAccessToken(Guid userId, string email, IEnumerable<string> roles);

    IEnumerable<Claim> BuildClaims(Guid userId, string email, IEnumerable<string> roles);
}