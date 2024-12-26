namespace jobApplicationTrackerApi.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Reflection;

internal class AuthResponsesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.ApiDescription.TryGetMethodInfo(out var methodInfo);

        if (methodInfo is null) return;

        var hasAuthorizeAttribute = false;

        if (methodInfo.MemberType == MemberTypes.Method)
        {
            hasAuthorizeAttribute = methodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>()
                .Any() ?? false;

            hasAuthorizeAttribute = hasAuthorizeAttribute
                ? !methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any()
                : methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
        }

        if (!hasAuthorizeAttribute) return;

        var statusUnauthorizedKey = StatusCodes.Status401Unauthorized.ToString();

        if (operation.Responses.All(r => r.Key != statusUnauthorizedKey))
            operation.Responses.Add(statusUnauthorizedKey, new OpenApiResponse
            {
                Description = "Unauthorized"
            });

        var statusForbiddenKey = StatusCodes.Status403Forbidden.ToString();

        if (operation.Responses.All(r => r.Key != statusForbiddenKey))
            operation.Responses.Add(statusForbiddenKey, new OpenApiResponse
            {
                Description = "Forbidden"
            });

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            }
        };
    }
}