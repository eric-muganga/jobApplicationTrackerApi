using jobApplicationTrackerApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace jobApplicationTrackerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class JobAppControllerBase : ControllerBase
{
    //public string userId => HttpContextHelper.GetUserId(User);
    //protected string userEmail => HttpContextHelper.GetUserInfo(User, "Email");

    /// <summary>
    /// Helping function to manage response types 
    /// </summary>
    protected IActionResult GenerateResponse<T>(ServiceResponse<T> response)
    {
        if (!response.Success)
            return StatusCode(Convert.ToInt32(response.StatusCode), response);

        if (response.StatusCode.Equals(HttpStatusCode.Created))
            return StatusCode(Convert.ToInt32(response.StatusCode), response);

        return Ok(response);
    }
}
