using DMIS_Backend.Api.Abstractions;
using DMIS_Backend.Application.Modules.Products.Commands.CreateProduct;

namespace DMIS_Backend.Api.Modules.Products.CreateProduct;

/// <summary>
/// 建立產品的 API Request DTO
/// 實作 ICommand 介面，提供 ToCommand() 轉換方法
/// </summary>
public sealed record CreateProductRequest(
  string Name,
  string Description,
  decimal BasePrice,
  string Currency = "TWD"
) : ICommand<CreateProductCommand>
{
  /// <summary>
  /// 將 API DTO 轉換為 Application Command
  /// </summary>
  public CreateProductCommand ToCommand() =>
    new()
    {
      Name = Name,
      Description = Description,
      BasePrice = BasePrice,
      Currency = Currency,
    };
}
