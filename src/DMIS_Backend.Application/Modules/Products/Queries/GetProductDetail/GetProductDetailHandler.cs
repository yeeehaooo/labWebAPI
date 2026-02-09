using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Kernel.Guards;
using DMIS_Backend.Application.Kernel.Results;

namespace DMIS_Backend.Application.Modules.Products.Queries.GetProductDetail;

/// <summary>
/// 取得產品詳情的處理器（CQRS Query 側）
/// 使用 ReadModel 直接投影成 DTO，不建立 Domain Model
/// </summary>
public class GetProductDetailHandler : IUseCaseQueryHandler<GetProductDetailQuery, GetProductDetailResult>
{
  private readonly IProductReadModel _productReadModel;

  public GetProductDetailHandler(IProductReadModel productReadModel)
  {
    _productReadModel = productReadModel;
  }

  public async Task<Result<GetProductDetailResult>> HandleAsync(GetProductDetailQuery query, CancellationToken ct)
  {
    try
    {
      // 使用 ReadModel 直接取得產品詳情（不建立 Domain Model）
      var product = await _productReadModel.GetProductDetailAsync(query.ProductId, ct);

      if (product == null)
      {
        AppGuard.Against.ThrowIf(product == null, Domain.Kernel.ErrorCodes.ErrorCode.ProductNotFound);
      }

      var result = new GetProductDetailResult
      {
        Product = product
      };

      return Result<GetProductDetailResult>.Success(result);
    }
    catch (Exception ex)
    {
      return Result<GetProductDetailResult>.Failure("", "", default!);
    }
  }
}
