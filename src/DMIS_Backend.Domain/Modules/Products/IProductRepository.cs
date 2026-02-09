using DMIS_Backend.Domain.Kernel;

namespace DMIS_Backend.Domain.Modules.Products;

/// <summary>
/// Product Repository 介面
/// 繼承泛型 IRepository，確保只操作 Aggregate Root
/// </summary>
public interface IProductRepository : IRepository<Product>
{
  /// <summary>
  /// 根據 ProductId（業務識別碼）取得 Product
  /// 這是 Product 特有的查詢方法，使用業務識別碼而非資料庫主鍵
  /// </summary>
  Task<Product?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
}
