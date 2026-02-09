using DMIS_Backend.Application.Kernel.Abstractions;

namespace DMIS_Backend.Application.Modules.Products.Queries.GetProductDetail;

/// <summary>
/// 取得產品詳情的查詢
/// </summary>
public record GetProductDetailQuery : IUseCaseQuery<GetProductDetailResult>
{
  public Guid ProductId { get; init; }
}

/// <summary>
/// 取得產品詳情的結果
/// </summary>
public record GetProductDetailResult
{
  public ProductDetailDto? Product { get; init; }
}
