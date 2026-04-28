using BasketService.Domain.Entities;
using BasketService.Domain.Repositories;
using MediatR;

namespace BasketService.Application.Commands;

public record UpdateBasketCommand(Basket Basket) : IRequest<Basket>;

public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, Basket>
{
    private readonly IBasketRepository _repository;

    public UpdateBasketCommandHandler(IBasketRepository repository)
    {
        _repository = repository;
    }

    public async Task<Basket> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        return await _repository.UpdateBasketAsync(request.Basket);
    }
}
