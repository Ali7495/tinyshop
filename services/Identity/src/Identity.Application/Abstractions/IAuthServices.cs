public interface IAuthServices
{
    Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
    Task<AuthResponse> RefreshAsync(RefreshRequest refreshRequest);
}