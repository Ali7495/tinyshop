public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task SaveRefreshToken(Guid userId, string refreshToken, DateTime expiresAt);
    Task<(Guid userId, string refreshToken, DateTime expiresAt)?> GetRefreshTokenAsync(string refreshToken);
    Task DeleteRefreshToken(string refreshToken);
}