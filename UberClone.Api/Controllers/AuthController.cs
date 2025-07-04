using Microsoft.AspNetCore.Mvc;
using UberClone.Application.DTOs;
using UberClone.Application.Interfaces.UseCases;

namespace UberClone.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRegisterUserCommand _registerUserCommand;

    public AuthController(IRegisterUserCommand registerUserCommand)
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