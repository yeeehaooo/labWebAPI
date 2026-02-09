using DMIS_Backend.Application.Modules.Products.Queries.GetProductDetail;

namespace DMIS_Backend.Application.Modules.Products.Queries;

/// <summary>
/// Product Read Model 介面
/// 定義複雜查詢和讀取操作的契約（CQRS Read Model）
/// </summary>
public interface IProductReadModel
{
  ///// <summary>
  ///// 取得產品列表（包含分頁）
  ///// </summary>
  //Task<(IEnumerable<ProductListItemDto> Products, int TotalCount)> GetProductsAsync(
  //  int pageNumber,
  //  int pageSize,
  //  CancellationToken cancellationToken = default
  //);

  /// <summary>
  /// 根據 ProductId 取得產品詳情
  /// </summary>
  Task<ProductDetailDto?> GetProductDetailAsync(
    Guid productId,
    CancellationToken cancellationToken = default
  );

  ///// <summary>
  ///// 根據條件搜尋產品
  ///// </summary>
  //Task<IEnumerable<ProductListItemDto>> SearchProductsAsync(
  //  string? searchTerm,
  //  decimal? minPrice,
  //  decimal? maxPrice,
  //  CancellationToken cancellationToken = default
  //);
}
