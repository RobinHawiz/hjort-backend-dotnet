using Microsoft.AspNetCore.Mvc;

namespace HjortApi.Setups;

public static class JsonSetup
{
    public static void ConfigureJson(this JsonOptions opts)
    {
        opts.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        opts.JsonSerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    }
}
