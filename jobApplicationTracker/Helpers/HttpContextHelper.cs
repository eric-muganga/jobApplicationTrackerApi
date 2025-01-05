namespace jobApplicationTrackerApi.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

public static class HttpContextHelper
{
    public static string GetUserId(IPrincipal? user)
    {
        string userId = Guid.Empty.ToString();

        if (user?.Identity is not ClaimsIdentity identity)
        {
            return userId;
        }

        var claim = GetClaim(identity, c => c.Type == ClaimTypes.NameIdentifier);

        return claim?.Value ?? userId;
    }

    //public static IReadOnlyCollection<T> GetUserRoles<T>(ClaimsPrincipal? user) where T : struct, Enum
    //{
    //    var userRoles = new Collection<T>();
    //    var identity = user?.Identity as ClaimsIdentity;

    //    var userRoleClaims = identity?.Claims.Where(c => c.Type == ClaimTypes.Role);

    //    if (userRoleClaims == null)
    //    {
    //        return userRoles;
    //    }

    //    foreach (var roleClaim in userRoleClaims)
    //    {
    //        if (Enum.TryParse(roleClaim.Value, out T userRole))
    //        {
    //            userRoles.Add(userRole);
    //        }
    //    }

    //    return userRoles;
    //}

    public static string GetUserInfo(ClaimsPrincipal? user, string type)
    {
        var returnString = string.Empty;
        var identity = user?.Identity as ClaimsIdentity;
        var claim = type switch
        {
            "Email" => GetClaim(identity, c => c.Type == ClaimTypes.Email),
            _ => null
        };

        if (claim != null)
        {
            returnString = claim.Value;
        }

        return returnString;
    }

    private static Claim? GetClaim(ClaimsIdentity? identity, Func<Claim, bool> expression)
    {
        return identity?.Claims.FirstOrDefault(expression);
    }
}
