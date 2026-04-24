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

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasket(string userId)
    {
        var query = new GetBasketQuery(userId);
        var basket = await _mediator.Send(query);

        if (basket is null)
            return NotFound();

        return Ok(basket);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateBasket([FromBody] Basket basket)
    {
        var command = new UpdateBasketCommand(basket);
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteBasket(string userId)
    {
        var command = new DeleteBasketCommand(userId);
        await _mediator.Send(command);

        return NoContent();
    }
}