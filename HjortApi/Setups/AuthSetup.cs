using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HjortApi.Setups;

public static class AuthSetup
{
    public static void AddAuth(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication("Bearer").AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config.GetValue<string>("Authentication:Issuer"),
                ValidAudience = config.GetValue<string>("Authentication:Audience"),
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(
                    config.GetValue<string>("Authentication:SecretKey") ?? throw new InvalidOperationException("Authentication:SecretKey is missing.")))
            };
        });
    }
}
