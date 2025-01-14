namespace jobApplicationTrackerApi.Helpers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

using System.Text;

public static class JwtAuthenticationExtension
{
    public static AuthenticationBuilder AddJwtAuthentication(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]));
        
        authenticationBuilder
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("a6e0cbea095e2e237f2fdd3586f6ddd557ac9d45db2672c8bbdb266d891c4958"));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return authenticationBuilder;
    }
}
