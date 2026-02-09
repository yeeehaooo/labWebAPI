namespace DMIS_Backend.Application.Modules.Products.Commands.CreateProductWithDapper;

/// <summary>
/// 建立產品的結果
/// </summary>
public class CreateProductWithDapperResult
{
  public Guid ProductId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public decimal BasePrice { get; set; }
  public DateTime CreatedAt { get; set; }
}
