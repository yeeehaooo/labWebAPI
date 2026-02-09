using DMIS_Backend.Application.Modules.Products.Commands.CreateProduct;

namespace DMIS_Backend.Api.Modules.Products.CreateProduct;

/// <summary>
/// CreateProduct 模組的映射擴充方法
/// 提供 Application Result → API Response 的轉換
/// </summary>
public static class CreateProductMapping
{
  /// <summary>
  /// Application CreateProductResult → API CreateProductResponse
  /// </summary>
  public static CreateProductResponse ToCreateProductResponse(this CreateProductResult result) =>
    new(
      ProductId: result.ProductId,
      Name: result.Name,
      Description: result.Description,
      BasePrice: result.BasePrice,
      CreatedAt: result.CreatedAt
    );
}
