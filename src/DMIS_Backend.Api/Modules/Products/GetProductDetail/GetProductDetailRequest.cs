using DMIS_Backend.Application.Modules.Products.Queries.GetProductDetail;

namespace DMIS_Backend.Api.Modules.Products.GetProductDetail;

/// <summary>
/// 取得產品詳情的 API Request DTO
/// </summary>
public sealed record GetProductDetailRequest
{
  /// <summary>
  /// 將 API DTO 轉換為 Application Query
  /// </summary>
  public GetProductDetailQuery ToQuery(Guid productId) => new() { ProductId = productId };
}
