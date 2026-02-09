using DMIS_Backend.Application.Kernel.Abstractions;

namespace DMIS_Backend.Application.Modules.Products.Commands.CreateProductWithDapper;

/// <summary>
/// 使用 Dapper 建立產品的命令
/// </summary>
public class CreateProductWithDapperCommand : IUseCaseCommand<CreateProductWithDapperResult>
{
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public decimal BasePrice { get; set; }
}
