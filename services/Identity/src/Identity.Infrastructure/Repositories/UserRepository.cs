
using Dapper;
using Npgsql;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private NpgsqlConnection GetConnection()
    {
        return new(_connectionString);
    }


    public async Task AddAsync(User user)
    {
        string sql = @"INSERT INTO users (id, email, passwordHash, role)
                       VALUES (@Id, @Email, @PasswordHash, @Role)";

        using NpgsqlConnection connection = GetConnection();

        await connection.ExecuteAsync(sql, new
        {
            user.Id,
            user.Email,
            user.PasswordHash,
            user.Roles
        });
    }

    public async Task DeleteRefreshToken(string refreshToken)
    {
        string sql = @"DELETE refresh_tokens
                       WHERE refreshToken = @RefreshToken";

        NpgsqlConnection connection = GetConnection();

        await connection.ExecuteAsync(sql, new
        {
            RefreshToken = refreshToken
        });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        string sql = @"SELECT id, email, passwordHash, role
                       FROM users
                       WHERE email = @Email";

        using NpgsqlConnection connection = GetConnection();

        return await connection.QuerySingleOrDefaultAsync<User>(
            sql, new { Email = email }
        );  
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        string sql = @"SELECT id, email, passwordHash, role
                       FROM users
                       WHERE id = @Id";
        using NpgsqlConnection connection = GetConnection();

        return await connection.QuerySingleOrDefaultAsync<User>(
            sql, new { Id = id }
        );
    }

    public async Task<(Guid userId, string refreshToken, DateTime expiresAt)?> GetRefreshTokenAsync(string refreshToken)
    {
        string sql = @"SELECT userId, refreshToken, expiresAt
                       FROM refresh_tokens
                       WHERE refreshToken = @RefreshToken";

        using NpgsqlConnection connection = GetConnection();

        var result = await connection.QuerySingleOrDefaultAsync<(Guid, string, DateTime)>(sql, new
        {
            RefreshToken = refreshToken
        });

        return result == default ? null : result;
    }

    public async Task SaveRefreshToken(Guid userId, string refreshToken, DateTime expiresAt)
    {
        string sql = @"INSERT INTO refresh_tokens (userId, refreshToken, expiresAt)
                       VALUES (@UserId, @RefreshToken, @ExpiresAt)";

        using NpgsqlConnection connection = GetConnection();
        await connection.ExecuteAsync(sql, new
        {
            UserId = userId,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt
        });


    }
}