using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Application.Wrappers;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Commands.UpdateProduct;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, ApiResponse<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProductDto>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
            return ApiResponse<ProductDto>.Fail($"Ürün bulunamadı. Id: {request.Id}");

        product.Update(request.Name, request.Description, request.Price, request.Stock);

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<ProductDto>(product);
        return ApiResponse<ProductDto>.Ok(dto, "Ürün başarıyla güncellendi.");
    }
}
