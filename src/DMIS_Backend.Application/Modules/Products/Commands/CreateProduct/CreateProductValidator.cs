//using DMIS_Backend.Application.Kernel.Guards;
//using DMIS_Backend.Application.Kernel.Results;

//namespace DMIS_Backend.Application.Modules.Products.Commands.CreateProduct;

///// <summary>
///// CreateProductCommand 驗證器
///// </summary>
//public class CreateProductValidator
//{
//  private const int MaxNameLength = 200;
//  private const int MaxDescriptionLength = 1000;
//  private const decimal MinBasePrice = 0;

//  /// <summary>
//  /// 驗證命令
//  /// </summary>
//  public Result Validate(CreateProductCommand command)
//  {
//    var validationFailures = new List<ValidationError>();

//    // 驗證產品名稱
//    if (string.IsNullOrWhiteSpace(command.Name))
//    {
//      validationFailures.Add(new ValidationFailure(
//          nameof(command.Name),
//          "Product name is required.",
//          command.Name));
//    }
//    else if (command.Name.Length > MaxNameLength)
//    {
//      validationFailures.Add(new ValidationFailure(
//          nameof(command.Name),
//          $"Product name cannot exceed {MaxNameLength} characters.",
//          command.Name));
//    }

//    // 驗證描述
//    if (!string.IsNullOrWhiteSpace(command.Description) && command.Description.Length > MaxDescriptionLength)
//    {
//      validationFailures.Add(new ValidationFailure(
//          nameof(command.Description),
//          $"Product description cannot exceed {MaxDescriptionLength} characters.",
//          command.Description));
//    }

//    // 驗證價格
//    if (command.BasePrice < MinBasePrice)
//    {
//      validationFailures.Add(new ValidationFailure(
//          nameof(command.BasePrice),
//          $"Product base price must be greater than or equal to {MinBasePrice}.",
//          command.BasePrice));
//    }

//    // 驗證貨幣
//    if (string.IsNullOrWhiteSpace(command.Currency))
//    {
//      validationFailures.Add(new ValidationFailure(
//          nameof(command.Currency),
//          "Currency is required.",
//          command.Currency));
//    }

//    if (validationFailures.Any())
//    {
//      return Result.ValidationFailure(validationFailures, "Validation failed for create product command.");
//    }

//    return Result.Success();
//  }
//}
