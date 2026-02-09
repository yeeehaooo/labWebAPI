using DMIS_Backend.Domain.Kernel;

namespace DMIS_Backend.Domain.Modules.Products;

/// <summary>
/// Product 領域實體（Aggregate Root）
/// 對應資料表：Products
/// </summary>
public class Product : AggregateRoot
{
  public int Id { get; private set; }
  public Guid ProductId => AggregateId; // 業務識別碼（對應 AggregateId）
  public ProductName Name { get; private set; } = null!;
  public string Description { get; private set; } = string.Empty;
  public decimal BasePrice { get; private set; }
  public DateTime CreatedAt { get; private set; }
  public DateTime? UpdatedAt { get; private set; }

  // 私有建構函式（用於 Dapper/ORM 映射）
  private Product() { }

  /// <summary>
  /// 建立新產品
  /// </summary>
  public Product(string name, string description, decimal basePrice)
    : base(Guid.NewGuid()) // 呼叫 AggregateRoot 建構函式，設定 AggregateId
  {
    Name = new ProductName(name);
    Description = description;
    BasePrice = basePrice;
    CreatedAt = DateTime.Now;
  }
  /// <summary>
  /// 更新產品名稱
  /// </summary>
  public void UpdateName(string name)
  {
    Name = new ProductName(name);
    UpdatedAt = DateTime.Now;
  }

  /// <summary>
  /// 更新產品描述
  /// </summary>
  public void UpdateDescription(string description)
  {
    Description = description;
    UpdatedAt = DateTime.Now;
  }

  /// <summary>
  /// 更新產品價格
  /// </summary>
  public void UpdateBasePrice(decimal basePrice)
  {
    BasePrice = basePrice;
    UpdatedAt = DateTime.Now;
  }

  /// <summary>
  /// 更新產品資訊
  /// </summary>
  public void Update(string? name, string? description, decimal? basePrice)
  {
    if (name != null)
    {
      UpdateName(name);
    }

    if (description != null)
    {
      UpdateDescription(description);
    }

    if (basePrice.HasValue)
    {
      UpdateBasePrice(basePrice.Value);
    }
  }
}
