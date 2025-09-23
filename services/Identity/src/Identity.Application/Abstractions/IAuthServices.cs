public interface IAuthServices
{
    Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest, List<string> roles);
    Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
    Task<AuthResponse> RefreshAsync(RefreshRequest refreshRequest);
}