using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.Reports.Queries;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController(IMediator mediator) : ControllerBase
{
    [HttpGet("stock-summary")]
    public async Task<IActionResult> StockSummary()
    {
        var result = await mediator.Send(new GetStockSummaryQuery());
        return Ok(result);
    }
}
