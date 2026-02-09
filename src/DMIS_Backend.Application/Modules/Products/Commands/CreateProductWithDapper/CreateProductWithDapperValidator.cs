//using DMIS_Backend.Application.Kernel.Guards;
//using DMIS_Backend.Application.Kernel.Results;

//namespace DMIS_Backend.Application.Modules.Products.Commands.CreateProductWithDapper;

///// <summary>
///// 建立產品命令的驗證器
///// </summary>
//public class CreateProductWithDapperValidator<T> where T : CreateProductWithDapperCommand
//{
//  public Result<T> Validate(T command)
//  {
//    var validations = new List<ValidationError>();
//    if (string.IsNullOrWhiteSpace(command.Name))
//    {
//      validations.Add(new ValidationError(nameof(command.Name), "PRODUCT_NAME_REQUIRED"));
//    }

//    if (command.Name.Length > 200)
//    {
//      validations.Add(new ValidationError(nameof(command.Name), "PRODUCT_NAME_TOO_LONG"));
//    }

//    if (command.BasePrice < 0)
//    {
//      validations.Add(new ValidationError(nameof(command.Name), "BASE_PRICE_INVALID"));
//    }

//    return validations;
//  }
//}
