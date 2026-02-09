using System.Data.Common;
using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Kernel.Results;
using DMIS_Backend.Domain.Kernel.ErrorCodes;
using DMIS_Backend.Domain.Modules.Products;

namespace DMIS_Backend.Application.Modules.Products.Commands.CreateProduct;

/// <summary>
/// 建立產品的處理器
/// </summary>
public class CreateProductCommanHandler : IUseCaseCommandHandler<CreateProductCommand, CreateProductResult>
{
  private readonly IProductRepository _productRepository;
  //private readonly CreateProductValidator _validator;

  public CreateProductCommanHandler(IProductRepository productRepository
    //, CreateProductValidator validator
    )
  {
    _productRepository = productRepository;
    //_validator = validator;
  }

  public async Task<Result<CreateProductResult>> HandleAsync(CreateProductCommand command, CancellationToken ct)
  {
    //// 驗證命令
    //var validationResult = _validator.Validate(command);
    //if (validationResult.IsFailure)
    //{
    //  return Result<CreateProductResult>.f(validationResult);
    //}

    try
    {
      // 建立領域實體
      var product = new Product(command.Name, command.Description, command.BasePrice);
      // 儲存到資料庫
      await _productRepository.AddAsync(product, ct);
      await _productRepository.SaveChangesAsync(ct);

      // 回傳結果
      var result = new CreateProductResult
      {
        ProductId = product.ProductId,
        Name = product.Name.Value,
        Description = product.Description,
        BasePrice = product.BasePrice,
        CreatedAt = product.CreatedAt
      };

      return Result<CreateProductResult>.Success(result);
    }
    catch (DbException ex)
    {
      return Result<CreateProductResult>.Failure(ErrorCode.DbError, ex);
    }
    catch (Exception ex)
    {
      return Result<CreateProductResult>.Failure(ErrorCode.SystemError, ex);
    }
  }
}
