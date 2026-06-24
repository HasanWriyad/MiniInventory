using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.Stock.Commands;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StockController(IMediator mediator) : ControllerBase
{
    public record StockInRequest(int ItemId, string BatchNumber, decimal Quantity, DateTime ExpiryDate);
    public record StockOutRequest(int ItemId, decimal Quantity);

    [HttpPost("in")]
    public async Task<IActionResult> StockIn([FromBody] StockInRequest request)
    {
        await mediator.Send(new StockInCommand(
            request.ItemId,
            request.BatchNumber,
            request.Quantity,
            request.ExpiryDate,
            GetCurrentUserId()));
        return NoContent();
    }

    [HttpPost("out")]
    public async Task<IActionResult> StockOut([FromBody] StockOutRequest request)
    {
        await mediator.Send(new StockOutCommand(
            request.ItemId,
            request.Quantity,
            GetCurrentUserId()));
        return NoContent();
    }

    private int GetCurrentUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
