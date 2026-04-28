using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Wrappers;

namespace ProductService.Application.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int Stock) : IRequest<ApiResponse<ProductDto>>;
