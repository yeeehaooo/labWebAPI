using DMIS_Backend.Application.Kernel.Abstractions;

namespace DMIS_Backend.Application.Modules.Products.Commands.CreateProduct;

/// <summary>
/// 建立產品的命令
/// </summary>
public record CreateProductCommand : IUseCaseCommand<CreateProductResult>
{
  public string Name { get; init; } = string.Empty;
  public string Description { get; init; } = string.Empty;
  public decimal BasePrice { get; init; }
  public string Currency { get; init; } = "TWD";
}

/// <summary>
/// 建立產品的結果
/// </summary>
public record CreateProductResult
{
  public Guid ProductId { get; init; }
  public string Name { get; init; } = string.Empty;
  public string Description { get; init; } = string.Empty;
  public decimal BasePrice { get; init; }
  public DateTime CreatedAt { get; init; }
}
