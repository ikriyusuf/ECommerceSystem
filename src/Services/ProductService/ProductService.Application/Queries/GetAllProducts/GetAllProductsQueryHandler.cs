using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Wrappers;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Queries.GetAllProducts;

public class GetAllProductsQueryHandler
    : IRequestHandler<GetAllProductsQuery, ApiResponse<IReadOnlyList<ProductDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IReadOnlyList<ProductDto>>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.Products.GetAllAsync(cancellationToken);
        
        var dtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);

        return ApiResponse<IReadOnlyList<ProductDto>>.Ok(dtos);
    }
}
