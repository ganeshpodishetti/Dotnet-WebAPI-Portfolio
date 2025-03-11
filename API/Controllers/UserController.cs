using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserServices userServices) : Controller
{
    [HttpGet]
    public IActionResult GetUserProfiles()
    {
        var result = userServices.GetProfileAsync();
        return Ok(result);
    }
}