using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Wrappers;

namespace ProductService.Application.Queries.GetAllProducts;

public record GetAllProductsQuery()
    : IRequest<ApiResponse<IReadOnlyList<ProductDto>>>;
