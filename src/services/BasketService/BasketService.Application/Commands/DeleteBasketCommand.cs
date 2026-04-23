using BasketService.Domain.Repositories;
using MediatR;

namespace BasketService.Application.Commands;

public record DeleteBasketCommand(string Id) : IRequest;

public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand>
{
    private readonly IBasketRepository _repository;

    public DeleteBasketCommandHandler(IBasketRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteBasketAsync(request.Id);
    }
}
