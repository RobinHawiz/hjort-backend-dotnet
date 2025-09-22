using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace HjortApi.Setups;

public static class SwaggerSetup
{
    public static void ConfigureSwagger(this SwaggerGenOptions opts)
    {
        string xmlFile = $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
        opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
    }
}
