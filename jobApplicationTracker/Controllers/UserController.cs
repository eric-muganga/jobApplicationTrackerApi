using AutoMapper;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.Services;
using jobApplicationTrackerApi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jobApplicationTrackerApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager
    ) : JobAppControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(
        string email,
        string password,
        string firstName,
        string fullName)
    {
        var nUser = new ApplicationUser()
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            FullName = fullName
        };
        var result = await userManager.CreateAsync(nUser, password);
        if (result.Succeeded)
        {
            return Ok(nUser);
        }
        return BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel login)
    {
        var user = await SignInAsync(login.Email, login.Password);
        if (user != null)
        {
            var model = new UserMainInfoViewModel()
            {
                Id = new Guid(user.Id),
                Email = login.Email,
                RoleId = RoleType.NoRole
            };

            var encodedJwt = GetJwtToken(model);
            var response = new { token = encodedJwt };

            return Ok(response);
        }
        return Unauthorized();
    }

    // TODO: move to service
    private async Task<ApplicationUser?> SignInAsync(string email, string password)
    {
        var user = userManager.Users.FirstOrDefault(o => o.Email == email);
        if (user != null)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                return user;
            }

            throw new Exception("Can't login");
        }
        throw new Exception($"User with email={email} was not found");
    }

    private static string GetJwtToken(UserMainInfoViewModel userMainInfo)
    {
        var now = DateTime.UtcNow;
        var lifetime = TimeSpan.FromMinutes(45);

        var claims = new Collection<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userMainInfo.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, userMainInfo.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new(JwtRegisteredClaimNames.Exp,
                new DateTimeOffset(now.Add(lifetime)).ToUniversalTime().ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            new(ClaimTypes.Role, userMainInfo.RoleId.ToString())
        };

        //TODO update secret key
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("a6e0cbea095e2e672c8bbdb266d891c4958237f2fdd3586f6ddd557ac9d45db2"));
        var jwt = new JwtSecurityToken("FlexBenefitsIssuer", "FlexBenefitsIssuer", claims,
            now, now.Add(lifetime), new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

}
