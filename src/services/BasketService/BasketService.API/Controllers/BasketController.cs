using BasketService.Application.Commands;
using BasketService.Application.Queries;
using BasketService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IMediator _mediator;

    public BasketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Create(Guid id)
    {
        var query = new GetBasketQuery(id);
        var basket = await _mediator.Send(query);

        if (basket is null)
            return NotFound();

        return Ok(basket);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Basket basket)
    {
        var command = new UpdateBasketCommand(basket);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var command = new DeleteBasketCommand(id);
        await _mediator.Send(command);

        return Ok();
    }
}