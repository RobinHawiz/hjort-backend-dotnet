namespace HjortApi.Endpoints;

internal static class UtilityEndpoints
{
    internal static void AddUtilityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth", AuthCheck).RequireAuthorization();

        app.MapGet("/api/health", HealthCheck);
    }

    private static IResult AuthCheck()
    {
        return Results.Ok();
    }

    private static IResult HealthCheck()
    {
        return Results.Ok("healthy");
    }
}
