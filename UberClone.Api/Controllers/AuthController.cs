using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs;
using UberClone.Application.UseCases.User;

namespace UberClone.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserCommand _registerUserCommand;

    public AuthController(RegisterUserCommand registerUserCommand)
    {
        _registerUserCommand = registerUserCommand;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            await _registerUserCommand.ExecuteAsync(dto);
            return Ok("User registered successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}