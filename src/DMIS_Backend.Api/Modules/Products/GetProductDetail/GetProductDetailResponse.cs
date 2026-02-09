namespace DMIS_Backend.Api.Modules.Products.GetProductDetail;

/// <summary>
/// 取得產品詳情的 API Response DTO
/// </summary>
public sealed record GetProductDetailResponse(
  int Id,
  Guid ProductId,
  string Name,
  string Description,
  decimal BasePrice,
  DateTime CreatedAt,
  DateTime? UpdatedAt
);
