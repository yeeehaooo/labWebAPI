using DMIS_Backend.Domain.Kernel.ErrorCodes;

namespace DMIS_Backend.Domain.Modules.Products;

/// <summary>
/// Product Domain 層錯誤分類入口
/// 提供語意化的錯誤碼引用，實際錯誤碼定義在 ErrorCode 中
/// </summary>
public static class ProductErrors
{
  /// <summary>
  /// 產品名稱不能為空
  /// </summary>
  public static ErrorCode NameCannotBeEmpty =>
      ErrorCode.NameCannotBeEmpty;

  /// <summary>
  /// 產品名稱長度超過限制
  /// </summary>
  public static ErrorCode NameTooLong =>
      ErrorCode.ProductNameTooLong;

  /// <summary>
  /// 產品名稱不能為空
  /// </summary>
  public static ErrorCode ProductNotFound =>
      ErrorCode.ProductNotFound;
}
