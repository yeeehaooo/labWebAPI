namespace DMIS_Backend.Api.Modules.Products.CreateProduct;

/// <summary>
/// 建立產品的 API Response DTO
/// </summary>
public sealed record CreateProductResponse(
  Guid ProductId,
  string Name,
  string Description,
  decimal BasePrice,
  DateTime CreatedAt
);
