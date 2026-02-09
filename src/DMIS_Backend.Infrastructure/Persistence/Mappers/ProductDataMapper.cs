using System.Reflection;
using DMIS_Backend.Domain.Modules.Products;
using DMIS_Backend.Infrastructure.Persistence.DataModels;

namespace DMIS_Backend.Infrastructure.Persistence.Mappers;

/// <summary>
/// ProductData 與 Product Domain Entity 之間的映射器
/// 確保 Aggregate 透過正確的建構函式重建，符合 DDD 原則
/// </summary>
public static class ProductDataMapper
{
  /// <summary>
  /// 將 ProductData 轉換為 Product Domain Entity
  /// 透過公開建構函式重建 Aggregate，確保驗證規則和 Value Object 正確建立
  /// </summary>
  public static Product ToDomain(this ProductData data)
  {
    // 使用公開建構函式建立 Product（會觸發驗證和 Value Object 建立）
    // 注意：建構函式會自動設定 AggregateId（對應 ProductId）
    var product = new Product(data.Name, data.Description, data.BasePrice);

    // 使用反射設定持久化相關的屬性（AggregateId, CreatedAt）
    // 這些屬性在 AggregateRoot 基類中定義
    var type = typeof(Product);
    var baseType = type.BaseType; // AggregateRoot

    // 設定 AggregateRoot 的屬性
    var aggregateIdProperty = baseType?.GetProperty(
      "AggregateId",
      BindingFlags.Public | BindingFlags.Instance
    );
    var createdAtProperty = baseType?.GetProperty(
      "CreatedAt",
      BindingFlags.Public | BindingFlags.Instance
    );

    aggregateIdProperty?.SetValue(product, data.ProductId); // ProductId 對應 AggregateId
    createdAtProperty?.SetValue(product, data.CreatedAt);

    return product;
  }

  /// <summary>
  /// 將 Product Domain Entity 轉換為 ProductData
  /// </summary>
  public static ProductData ToData(this Product domain)
  {
    return new ProductData
    {
      ProductId = domain.AggregateId, // AggregateId 對應 ProductId
      Name = domain.Name.Value,
      Description = domain.Description,
      BasePrice = domain.BasePrice,
      CreatedAt = domain.CreatedAt,
      UpdatedAt = null, // 新增時 UpdatedAt 為 null
    };
  }

  /// <summary>
  /// 將 Domain Entity 的變更套用到 ProductData
  /// 用於 Update 操作，確保由 Domain 驅動 Data 的更新
  /// </summary>
  public static void Apply(this ProductData data, Product domain)
  {
    data.Name = domain.Name.Value;
    data.Description = domain.Description;
    data.BasePrice = domain.BasePrice;
    // 注意：Id, ProductId, CreatedAt, UpdatedAt 不應該被更新
  }
}
