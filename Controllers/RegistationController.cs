// Controller which handles the registration of new users
using IBUAPI.Models;
using IBUAPI.Models.Dto;
using IBUAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace IBUAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController : ControllerBase
{
    private readonly IUserRegistration _registrationService;
    public RegistrationController(IUserRegistration registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost("RegisterUser")]
    public async Task<ActionResult<User>> RegisterUser(UserRegistrationDto userToReg)
    {
        try
        {
            return Ok(await _registrationService.RegisterUser(userToReg));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("Login")]
    public ActionResult<string> Login(UserLoginDto user)
    {
        try
        {
            return Ok(_registrationService.Login(user));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
