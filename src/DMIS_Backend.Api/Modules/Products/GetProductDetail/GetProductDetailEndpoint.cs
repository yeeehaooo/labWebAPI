using DMIS_Backend.Api.Common;
using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Modules.Products.Queries.GetProductDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMIS_Backend.Api.Modules.Products.GetProductDetail;

/// <summary>
/// 取得產品詳情端點
/// Vertical Slice 架構：每個 Action 擁有獨立的 Endpoint
/// </summary>
[ApiController]
[Tags("Product")]
[Route("api/products")]
public class GetProductDetailEndpoint : ControllerBase
{
  /// <summary>
  /// 取得產品詳情
  /// </summary>
  [HttpGet("{productId}")]
  [AllowAnonymous]
  [ProducesResponseType(typeof(APIResponse<GetProductDetailResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(
    typeof(APIResponse<GetProductDetailResponse>),
    StatusCodes.Status404NotFound
  )]
  public async Task<IActionResult> Handle(
    [FromRoute] Guid productId,
    [FromServices] IUseCaseQueryHandler<GetProductDetailQuery, GetProductDetailResult> handler,
    CancellationToken cancellationToken
  )
  {
    // 將 API Request 轉換為 Application Query
    var query = new GetProductDetailQuery { ProductId = productId };

    // 呼叫 UseCase Handler
    var result = await handler.HandleAsync(query, cancellationToken);

    // 將 Application Result 轉換為 API Response
    var apiResult = result.Map(data => data.ToGetProductDetailResponse());
    return Ok(apiResult);
  }
}
