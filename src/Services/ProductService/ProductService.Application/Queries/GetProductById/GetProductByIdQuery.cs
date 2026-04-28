using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Wrappers;

namespace ProductService.Application.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ApiResponse<ProductDto>>;
