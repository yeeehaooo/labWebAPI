namespace DMIS_Backend.Application.Modules.Products.Queries.GetProductDetail;

/// <summary>
/// Product Detail DTO (Read Model)
/// 用於產品詳情查詢的資料傳輸
/// </summary>
public class ProductDetailDto
{
  public int Id { get; set; }
  public Guid ProductId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public decimal BasePrice { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
