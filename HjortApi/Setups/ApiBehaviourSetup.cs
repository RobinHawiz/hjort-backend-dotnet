using HjortApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HjortApi.Setups;

public static class ApiBehaviourSetup
{
    public static void ConfigureModelState(this ApiBehaviorOptions opts)
    {
        opts.InvalidModelStateResponseFactory = actioncontext =>
        {
            List<ErrorResponse> errors = new();
            foreach (var modelStateEntry in actioncontext.ModelState)
            {
                string key = modelStateEntry.Key;
                // Skip entries with no errors (e.g., route params like `id` still appear in ModelState).
                if (key != "id")
                {
                    if (key == "Password")
                    {
                        // Rewrite so error response references the property the client actually sends, which is PasswordHash.
                        key = "PasswordHash";
                    }
                    ErrorResponse error = new(char.ToLower(key[0]) + key[1..], modelStateEntry.Value.Errors.Select(me => me.ErrorMessage).DefaultIfEmpty("").First());
                    errors.Add(error);
                }
            }
            return new BadRequestObjectResult(errors);
        };
    }
}
