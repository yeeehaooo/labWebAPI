using DMIS_Backend.Application.Modules.Products.Queries.GetProductDetail;

namespace DMIS_Backend.Api.Modules.Products.GetProductDetail;

/// <summary>
/// GetProductDetail 模組的映射擴充方法
/// 提供 Application Result → API Response 的轉換
/// </summary>
public static class GetProductDetailMapping
{
  /// <summary>
  /// Application GetProductDetailResult → API GetProductDetailResponse
  /// </summary>
  public static GetProductDetailResponse ToGetProductDetailResponse(this GetProductDetailResult result)
  {
    if (result.Product == null)
    {
      throw new InvalidOperationException("Product is null");
    }

    return new GetProductDetailResponse(
      Id: result.Product.Id,
      ProductId: result.Product.ProductId,
      Name: result.Product.Name,
      Description: result.Product.Description,
      BasePrice: result.Product.BasePrice,
      CreatedAt: result.Product.CreatedAt,
      UpdatedAt: result.Product.UpdatedAt
    );
  }
}
