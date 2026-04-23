using BasketService.Domain.Entities;
using BasketService.Domain.Repositories;
using MediatR;

namespace BasketService.Application.Queries;

public record GetBasketQuery(Guid Id) : IRequest<Basket?>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Basket?>
{
    private readonly IBasketRepository _repository;

    public GetBasketQueryHandler(IBasketRepository repository)
    {
        _repository = repository;
    }

    public async Task<Basket?> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetBasketAsync(request.Id);
    }
}
