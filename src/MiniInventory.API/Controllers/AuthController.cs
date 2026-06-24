using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.Auth.Commands;
using MiniInventory.Domain.Enums;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    public record RegisterRequest(string Username, string Password, UserRole Role);
    public record LoginRequest(string Username, string Password);

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var id = await mediator.Send(
            new RegisterCommand(request.Username, request.Password, request.Role));
        return CreatedAtAction(nameof(Register), new { id }, new { id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await mediator.Send(
            new LoginCommand(request.Username, request.Password));
        return Ok(result);
    }
}
