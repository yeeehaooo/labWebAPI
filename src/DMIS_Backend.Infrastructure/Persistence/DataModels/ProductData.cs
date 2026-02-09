namespace DMIS_Backend.Infrastructure.Persistence.DataModels;

/// <summary>
/// Product 資料模型（用於 EF Core 映射）
/// 對應資料表：Products
/// </summary>
public class ProductData
{
  public int Id { get; set; }
  public Guid ProductId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public decimal BasePrice { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
