using MediatR;
using ProductService.Application.Wrappers;

namespace ProductService.Application.Commands.AddProductToBasket;

public record AddProductToBasketCommand(Guid ProductId, string UserId, int Quantity) : IRequest<ApiResponse<bool>>;
