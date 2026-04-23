using MediatR;
using ProductService.Application.Wrappers;

namespace ProductService.Application.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<ApiResponse<bool>>;
