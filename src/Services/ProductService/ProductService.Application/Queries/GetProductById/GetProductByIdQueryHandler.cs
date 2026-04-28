using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Wrappers;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Queries.GetProductById;

public class GetProductByIdQueryHandler
    : IRequestHandler<GetProductByIdQuery, ApiResponse<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProductDto>> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return ApiResponse<ProductDto>.Fail($"Ürün bulunamadı. Id: {request.Id}");

        var dto = _mapper.Map<ProductDto>(product);
        return ApiResponse<ProductDto>.Ok(dto);
    }
}
