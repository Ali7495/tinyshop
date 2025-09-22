using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public sealed class JwtTokenServices : ITokenServices
{
    private readonly JwtOptions _jwtOptions;
    private readonly byte[] _keyBytes;

    public JwtTokenServices(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
        _keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.Key);
    }

    public IEnumerable<Claim> BuildClaims(Guid userId, string email, IEnumerable<string> roles)
    {
        List<Claim> claims = new()
        {
            new Claim(JwtRegisteredClaimNames.Sub,userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        return claims;
    }

    public string GenerateAccessToken(Guid userId, string email, IEnumerable<string> roles)
    {
        throw new NotImplementedException();
    }
}