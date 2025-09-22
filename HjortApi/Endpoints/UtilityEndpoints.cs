namespace HjortApi.Endpoints;

internal static class UtilityEndpoints
{
    internal static void AddUtilityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth", AuthCheck)
           .RequireAuthorization()
           .WithTags("Utility")
           .WithSummary("Verifies that the caller is authenticated.")
           .WithDescription("Returns 200 OK when the provided JWT is valid and not expired. " +
                            "If the token is missing/invalid/expired the authentication middleware returns 401.")
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status401Unauthorized);

        app.MapGet("/api/health", HealthCheck)
           .WithTags("Utility")
           .WithSummary("Simple liveness check.")
           .WithDescription("Returns 200 OK with the string \"healthy\".")
           .Produces<string>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status500InternalServerError);
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
