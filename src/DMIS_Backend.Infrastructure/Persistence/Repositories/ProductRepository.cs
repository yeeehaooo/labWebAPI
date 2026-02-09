using DMIS_Backend.Domain.Modules.Products;
using DMIS_Backend.Infrastructure.Persistence.DataModels;
using DMIS_Backend.Infrastructure.Persistence.DbContexts;
using DMIS_Backend.Infrastructure.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DMIS_Backend.Infrastructure.Persistence.Repositories;

/// <summary>
/// Product Repository 的 EF Core 實作
/// 確保 Aggregate 透過正確的建構函式重建，符合 DDD 原則
/// </summary>
public class ProductRepository : IProductRepository
{
  private readonly DmisDbContext _context;

  public ProductRepository(DmisDbContext context)
  {
    _context = context;
  }

  public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
  {
    var productData = await _context
      .Set<ProductData>()
      .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    return productData == null ? null : MapToDomain(productData);
  }

  public async Task<Product?> GetByAggregateIdAsync(
    Guid aggregateId,
    CancellationToken cancellationToken = default
  )
  {
    var productData = await _context
      .Set<ProductData>()
      .FirstOrDefaultAsync(p => p.ProductId == aggregateId, cancellationToken);

    return productData == null ? null : MapToDomain(productData);
  }

  public async Task<Product?> GetByProductIdAsync(
    Guid productId,
    CancellationToken cancellationToken = default
  )
  {
    // 委派給 GetByAggregateIdAsync，因為 ProductId 就是 AggregateId
    return await GetByAggregateIdAsync(productId, cancellationToken);
  }

  public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
  {
    var productsData = await _context.Set<ProductData>().ToListAsync(cancellationToken);

    return productsData.Select(MapToDomain);
  }

  public async Task<IEnumerable<Product>> GetByPageAsync(
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default
  )
  {
    var offset = (pageNumber - 1) * pageSize;

    var productsData = await _context
      .Set<ProductData>()
      .OrderBy(p => p.Id)
      .Skip(offset)
      .Take(pageSize)
      .ToListAsync(cancellationToken);

    return productsData.Select(MapToDomain);
  }

  public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
  {
    var productData = MapToData(product);
    await _context.Set<ProductData>().AddAsync(productData, cancellationToken);
  }

  public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
  {
    // 使用 AggregateId 查找（因為 AggregateRoot 沒有 Id 屬性）
    var productData = await _context
      .Set<ProductData>()
      .FirstOrDefaultAsync(p => p.ProductId == product.AggregateId, cancellationToken);

    if (productData == null)
    {
      throw new InvalidOperationException($"Product with AggregateId {product.AggregateId} not found.");
    }

    // 使用 Domain 驅動 Data 的更新（確保業務邏輯在 Domain 層執行）
    productData.Apply(product);

    _context.Set<ProductData>().Update(productData);
  }

  public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default)
  {
    var productData = await _context
      .Set<ProductData>()
      .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    if (productData == null)
    {
      return 0;
    }

    _context.Set<ProductData>().Remove(productData);
    return 1;
  }

  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return _context.SaveChangesAsync(cancellationToken);
  }

  // Domain 實體與 Data Model 之間的映射（顯式重建 Aggregate）
  // 確保透過公開建構函式重建，觸發驗證規則和 Value Object 正確建立
  private static Product MapToDomain(ProductData data)
  {
    return data.ToDomain();
  }

  private static ProductData MapToData(Product product)
  {
    return product.ToData();
  }
}
