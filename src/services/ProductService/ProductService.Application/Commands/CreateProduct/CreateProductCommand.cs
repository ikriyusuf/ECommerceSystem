using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Wrappers;

namespace ProductService.Application.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock) : IRequest<ApiResponse<ProductDto>>;
