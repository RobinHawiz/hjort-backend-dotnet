namespace HjortApi.Setups;

public static class CorsSetup
{
    const string CorsPolicy = "corsPolicy";

    public static void AddCorsPolicies(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, policy =>
            {
                policy.WithOrigins(config.GetValue<string>("Cors:Origin")).AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    public static void UseCorsSetup(this IApplicationBuilder app)
    {
        app.UseCors(CorsPolicy);
    }
}
