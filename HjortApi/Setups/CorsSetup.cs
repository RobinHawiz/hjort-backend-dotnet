namespace HjortApi.Setups;

public static class CorsSetup
{
    const string ProdPolicy = "Prod";

    public static void AddCorsPolicies(this IServiceCollection services)
    {
        string corsPolicy = ProdPolicy;

        services.AddCors(options =>
        {
            options.AddPolicy(corsPolicy, policy =>
            {
                policy.WithOrigins("https://robinhawiz.github.io").AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    public static void UseCorsSetup(this IApplicationBuilder app)
    {
        app.UseCors(ProdPolicy);
    }
}
