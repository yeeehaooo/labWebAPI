using DMIS_Backend.Api.Common;
using DMIS_Backend.Api.Common.Workflows.IdentifierCodes;
using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Modules.Products.Commands.CreateProduct;
using DMIS_Backend.Domain.Kernel.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMIS_Backend.Api.Modules.Products.CreateProduct;

/// <summary>
/// 建立產品端點
/// Vertical Slice 架構：每個 Action 擁有獨立的 Endpoint
/// </summary>
[ApiController]
[Tags("Product")]
[Route("api/products")]
[ModuleCode(nameof(ModuleCode.DES))]
public class CreateProductEndpoint : ControllerBase
{
  /// <summary>
  /// 建立產品
  /// </summary>
  [HttpPost]
  [AllowAnonymous]
  [OperationType(nameof(OperationType.Command))]
  [ScopeFunction(nameof(ScopeFunction.DES020))]
  [ProducesResponseType(typeof(APIResponse<CreateProductResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(APIResponse<CreateProductResponse>), StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> Handle(
    [FromBody] CreateProductRequest request,
    [FromServices] IUseCaseCommandHandler<CreateProductCommand, CreateProductResult> handler,
    CancellationToken cancellationToken
  )
  {
    // 將 API Request 轉換為 Application Command
    var command = request.ToCommand();

    // 呼叫 UseCase Handler
    var result = await handler.HandleAsync(command, cancellationToken);

    // 只做 Result Data DTO mapping Response Data DTO 回傳結果
    return Ok(result.Map(data => data.ToCreateProductResponse()));
  }
}
