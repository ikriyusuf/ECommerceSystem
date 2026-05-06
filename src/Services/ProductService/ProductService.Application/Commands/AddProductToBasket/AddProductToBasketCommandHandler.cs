using MassTransit;
using MediatR;
using ProductService.Application.Wrappers;
using ProductService.Domain.Interfaces;
using Shared.Contracts.Events;

namespace ProductService.Application.Commands.AddProductToBasket;

public class AddProductToBasketCommandHandler : IRequestHandler<AddProductToBasketCommand, ApiResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public AddProductToBasketCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ApiResponse<bool>> Handle(AddProductToBasketCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            return ApiResponse<bool>.Fail("Ürün bulunamadı.");

        // RabbitMQ üzerinden event yayınla
        await _publishEndpoint.Publish<IAddToBasketEvent>(new
        {
            UserId = request.UserId,
            ProductId = product.Id,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = request.Quantity
        }, cancellationToken);

        return ApiResponse<bool>.Ok(true, "Ürün sepete ekleme talebi gönderildi.");
    }
}
