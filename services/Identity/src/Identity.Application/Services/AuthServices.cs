
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

public sealed class AuthServices : IAuthServices
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenServices _tokenServices;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public AuthServices(IUserRepository userRepository, ITokenServices tokenServices)
    {
        _userRepository = userRepository;
        _tokenServices = tokenServices;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
    {
        User user = await _userRepository.GetByEmailAsync(loginRequest.email) ?? throw new InvalidOperationException("Invalid credentials!");

        PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash.Value, loginRequest.password);
        if (result == PasswordVerificationResult.Failed)
            throw new InvalidOperationException("Invalid credentials!");

        return await IssueTokens(user);    
    }

    public async Task<AuthResponse> RefreshAsync(RefreshRequest refreshRequest)
    {
        var refreshData = await _userRepository.GetRefreshTokenAsync(refreshRequest.refreshToken) ?? throw new InvalidOperationException("Invalid refresh token");

        if (refreshData.expiresAt < DateTime.Now)
        {
            throw new InvalidOperationException("The refresh token has expired!");
        }

        User user = await _userRepository.GetByIdAsync(refreshData.userId) ?? throw new InvalidOperationException("User not found!");

        await _userRepository.DeleteRefreshToken(refreshData.refreshToken);

        return await IssueTokens(user);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest, List<string> roles)
    {
        Email email = Email.Create(registerRequest.email);
        User? existingUser = await _userRepository.GetByEmailAsync(registerRequest.email);

        if (existingUser is not null)
            throw new InvalidOperationException("The user is already exist!");

        PasswordHash passwordHash = PasswordHash.FromHashed(_passwordHasher.HashPassword(null!, registerRequest.password));

        User user = User.Register(email, passwordHash, roles);

        await _userRepository.AddAsync(user);

        return await IssueTokens(user);
    }




    private async Task<AuthResponse> IssueTokens(User user)
    {
        string accessToken = _tokenServices.GenerateAccessToken(user.Id, user.Email, user.Roles);
        string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        DateTime expiresAt = DateTime.Now.AddMinutes(15);

        await _userRepository.SaveRefreshToken(user.Id, refreshToken, expiresAt);

        return new(accessToken, refreshToken, expiresAt);
    }
}