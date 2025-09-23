using Microsoft.Extensions.DependencyInjection;
public sealed class DependencyInjection
{
    public static void RegisterServices(IServiceCollection service)
    {
        service.AddScoped<ITokenServices, JwtTokenServices>();
    }
}