using DMIS_Backend.Application.Modules.Products.Queries;
using DMIS_Backend.Application.Modules.Products.Queries.GetProductDetail;
using DMIS_Backend.Infrastructure.Persistence.DataModels;
using DMIS_Backend.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DMIS_Backend.Infrastructure.Persistence.ReadModels;

/// <summary>
/// Product Read Model 實作 (CQRS Read Model)
/// 用於複雜查詢和讀取操作，不涉及領域邏輯
/// </summary>
public class ProductReadModel : IProductReadModel
{
  private readonly DmisDbContext _context;

  public ProductReadModel(DmisDbContext context)
  {
    _context = context;
  }

  ///// <summary>
  ///// 取得產品列表（包含分頁）
  ///// </summary>
  //public async Task<(IEnumerable<ProductListItemDto> Products, int TotalCount)> GetProductsAsync(
  //    int pageNumber,
  //    int pageSize,
  //    CancellationToken cancellationToken = default)
  //{
  //  var query = _context.Set<ProductData>().AsQueryable();

  //  var totalCount = await query.CountAsync(cancellationToken);

  //  var offset = (pageNumber - 1) * pageSize;
  //  var products = await query
  //      .OrderBy(p => p.Id)
  //      .Skip(offset)
  //      .Take(pageSize)
  //      .Select(p => new ProductListItemDto
  //      {
  //        Id = p.Id,
  //        ProductId = p.ProductId,
  //        Name = p.Name,
  //        Description = p.Description,
  //        BasePrice = p.BasePrice,
  //        CreatedAt = p.CreatedAt,
  //        UpdatedAt = p.UpdatedAt
  //      })
  //      .ToListAsync(cancellationToken);

  //  return (products, totalCount);
  //}

  /// <summary>
  /// 根據 ProductId 取得產品詳情
  /// </summary>
  public async Task<ProductDetailDto?> GetProductDetailAsync(
      Guid productId,
      CancellationToken cancellationToken = default)
  {
    var product = await _context
        .Set<ProductData>()
        .Where(p => p.ProductId == productId)
        .Select(p => new ProductDetailDto
        {
          Id = p.Id,
          ProductId = p.ProductId,
          Name = p.Name,
          Description = p.Description,
          BasePrice = p.BasePrice,
          CreatedAt = p.CreatedAt,
          UpdatedAt = p.UpdatedAt
        })
        .FirstOrDefaultAsync(cancellationToken);

    return product;
  }

  ///// <summary>
  ///// 根據條件搜尋產品
  ///// </summary>
  //public async Task<IEnumerable<ProductListItemDto>> SearchProductsAsync(
  //    string? searchTerm,
  //    decimal? minPrice,
  //    decimal? maxPrice,
  //    CancellationToken cancellationToken = default)
  //{
  //  var query = _context.Set<ProductData>().AsQueryable();

  //  if (!string.IsNullOrWhiteSpace(searchTerm))
  //  {
  //    query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
  //  }

  //  if (minPrice.HasValue)
  //  {
  //    query = query.Where(p => p.BasePrice >= minPrice.Value);
  //  }

  //  if (maxPrice.HasValue)
  //  {
  //    query = query.Where(p => p.BasePrice <= maxPrice.Value);
  //  }

  //  return await query
  //      .Select(p => new ProductListItemDto
  //      {
  //        Id = p.Id,
  //        ProductId = p.ProductId,
  //        Name = p.Name,
  //        Description = p.Description,
  //        BasePrice = p.BasePrice,
  //        CreatedAt = p.CreatedAt,
  //        UpdatedAt = p.UpdatedAt
  //      })
  //      .ToListAsync(cancellationToken);
  //}
}
