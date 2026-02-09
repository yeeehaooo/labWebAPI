using System.Data.Common;
using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Kernel.Persistence;
using DMIS_Backend.Application.Kernel.Results;
using DMIS_Backend.Domain.Modules.Products;

namespace DMIS_Backend.Application.Modules.Products.Commands.CreateProductWithDapper;

/// <summary>
/// 使用 Dapper + IUnitOfWork 建立產品的範例 Handler
/// 展示如何在 Application 層使用業務語意抽象（而非技術細節）
///
/// 核心原則：
/// - Application 層只關心「業務語意」：提交變更（CommitAsync）
/// - 不關心「技術細節」：如何開 DB transaction、如何 commit
/// - 使用 IUnitOfWork 抽象，而非直接依賴 IDbSession
/// </summary>
public class CreateProductWithDapperHandler
  : IUseCaseCommandHandler<CreateProductWithDapperCommand, CreateProductWithDapperResult>
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IProductRepository _productRepository;
  //private readonly CreateProductWithDapperValidator _validator;

  public CreateProductWithDapperHandler(
    IUnitOfWork unitOfWork,
    IProductRepository productRepository
  //CreateProductWithDapperValidator validator
  )
  {
    _unitOfWork = unitOfWork;
    _productRepository = productRepository;
    //_validator = validator;
  }

  public async Task<Result<CreateProductWithDapperResult>> HandleAsync(
    CreateProductWithDapperCommand command,
    CancellationToken ct
  )
  {
    //// 1. 驗證命令
    //var validationResult = _validator.Validate(command);
    //if (validationResult.IsFailure)
    //{
    //  return Result<CreateProductWithDapperResult>.FromResult(validationResult);
    //}

    try
    {
      // 2. 建立 Domain 實體（業務邏輯在 Domain 層）
      var product = new Product(
        command.Name,
        command.Description,
        command.BasePrice
      );

      // 3. 使用 Repository 儲存
      await _productRepository.AddAsync(product, ct);

      // 4. 提交變更（業務語意：這個 UseCase 的變更需要被提交）
      //    技術細節（如何開 transaction、如何 commit）由 Infrastructure 層處理
      await _unitOfWork.CommitAsync(ct);

      // 5. 回傳結果
      var result = new CreateProductWithDapperResult
      {
        ProductId = product.ProductId,
        Name = product.Name.Value,
        Description = product.Description,
        BasePrice = product.BasePrice,
        CreatedAt = product.CreatedAt,
      };

      return Result<CreateProductWithDapperResult>.Success(result);
    }
    catch (DbException ex)
    {
      // 錯誤處理：如果 Repository 或 UnitOfWork 內部有交易管理，
      // 錯誤時會自動回滾（由 Infrastructure 層處理）
      return Result<CreateProductWithDapperResult>.Failure("1234", ex.Message, null);
    }
  }
}
