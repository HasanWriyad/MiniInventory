using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.Application.Items.Commands;
using MiniInventory.Application.Items.Queries;

namespace MiniInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ItemsController(IMediator mediator) : ControllerBase
{
    public record CreateItemRequest(string Name, string SKU, string Unit, string Category);
    public record UpdateItemRequest(string Name, string SKU, string Unit, string Category);

    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? category = null,
        [FromQuery] string? search = null)
    {
        var result = await mediator.Send(new GetItemsListQuery(page, pageSize, category, search));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await mediator.Send(new GetItemByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateItemRequest request)
    {
        var id = await mediator.Send(
            new CreateItemCommand(request.Name, request.SKU, request.Unit, request.Category));
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemRequest request)
    {
        await mediator.Send(
            new UpdateItemCommand(id, request.Name, request.SKU, request.Unit, request.Category));
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteItemCommand(id));
        return NoContent();
    }
}
