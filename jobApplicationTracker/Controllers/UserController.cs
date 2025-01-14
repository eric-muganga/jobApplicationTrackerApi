using AutoMapper;
using Azure.Core;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.Services;
using jobApplicationTrackerApi.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using jobApplicationTrackerApi.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace jobApplicationTrackerApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IConfiguration configuration
    ) : JobAppControllerBase
{
    //[AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto model)
    {
        var nUser = new ApplicationUser()
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            FullName = model.FullName
        };
        var result = await userManager.CreateAsync(nUser, model.Password);
        if (result.Succeeded)
        {
            return Ok(nUser);
        }
        
        // Log the errors for debugging
        foreach (var error in result.Errors)
        {
            Console.WriteLine($"Code: {error.Code}, Description: {error.Description}");
        }
        return BadRequest(result);
    }

    //[AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel login)
    {
        try
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
                return Ok(new { token = encodedJwt, user = user });
            }

            return Unauthorized("Invalid login credentials");
        }
        catch (Exception ex)
        {
            if (ex.Message == "Incorrect password")
            {
                return Unauthorized("Incorrect password. Please try again.");
            }

            if (ex.Message.Contains("was not found"))
            {
                return NotFound(ex.Message); // If the email was not found
            }

            return BadRequest("An error occurred during login. Please try again.");
        }
    }


    [HttpPost("changePassword")]
    public async Task<IActionResult> changePassword([FromBody] ChangePasswordViewModel request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) ||
            string.IsNullOrEmpty(request.OldPassword) ||
            string.IsNullOrEmpty(request.NewPassword))
        {
            return BadRequest("Invalid input.");
        }

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var passwordCheck = await signInManager.CheckPasswordSignInAsync(user, request.OldPassword, false);
        if (!passwordCheck.Succeeded)
        {
            return Unauthorized("Old password is incorrect.");
        }

        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);

        if (result.Succeeded)
        {
            return Ok("Password changed successfully.");
        }

        return BadRequest("Failed to change password.");
    }


    //public async Task<bool> SendResetPasswordMailAsync(string email, ApplicationUser user, bool isWelcomeMail = false)
    //{
    //    if (user != null)
    //    {
    //        var token = await signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
    //        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(token);
    //        token = HttpUtility.UrlEncode(plainTextBytes);

    //        var resetUrl = _configuration.GetValue<string>("SomeUIBaseURL") + $"/reset-password/{token}";

    //        if (isWelcomeMail)
    //            await _mailService.SendMailWelcomeAsync(email, resetUrl);
    //        else
    //            await _mailService.SendMailForgotPasswordAsync(email, resetUrl);

    //        return true;
    //    }

    //    return false;
    //}

    //[HttpPost("resetPassword")]
    //public async Task<IResult> ResetPassword([FromBody] ResetPasswordViewModel resetPassword)
    //{
    //    string userId = string.Empty;
    //    if (resetPassword.Token != null && resetPassword.NewPassword != null)
    //    {
    //        var token = HttpUtility.UrlDecode(resetPassword.Token);
    //        var newPassword = HttpUtility.UrlDecode(resetPassword.NewPassword);
    //        var dataProtector = _dataProtectionProvider.CreateProtector("DataProtectorTokenProvider");

    //        var resetTokenArray = Convert.FromBase64String(token);
    //        var unprotectedResetTokenArray = dataProtector.Unprotect(resetTokenArray);
    //        using (var ms = new MemoryStream(unprotectedResetTokenArray))
    //        {
    //            using (var reader = new BinaryReader(ms))
    //            {
    //                reader.ReadInt64();
    //                userId = reader.ReadString();
    //            }
    //        }

    //        if (userId != null)
    //        {
    //            var user = await signInManager.UserManager.FindByIdAsync(userId);
    //            if (user != null)
    //            {
    //                var result = await signInManager.UserManager.ResetPasswordAsync(user, token, newPassword);
    //                if (result.Succeeded)
    //                    return Results.Ok();
    //            }
    //        }
    //    }

    //    return Results.BadRequest();
    //}

    //changeUsername

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

            // If the password is incorrect
            throw new Exception("Incorrect password");
        }
        throw new Exception($"User with email={email} was not found");
    }

    private string GetJwtToken(UserMainInfoViewModel userMainInfo)
    {
        var jwtSettings = configuration.GetSection("Jwt"); // Use IConfiguration to load settings
        var now = DateTime.UtcNow;
        var lifetime = TimeSpan.FromMinutes(int.Parse(jwtSettings["TokenExpiryMinutes"])); // Dynamically set token lifetime

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
            //new(ClaimTypes.Role, userMainInfo.RoleId.ToString())
        };

        //TODO update secret key
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]));
        var jwt = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            notBefore: now,
            expires: now.Add(lifetime),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

}
