using DMIS_Backend.Domain.Kernel.SmartEnums;

namespace DMIS_Backend.Domain.Kernel.ErrorCodes;


/// <summary>
/// 錯誤碼定義
/// 包含 Domain、Application、System 錯誤碼
/// </summary>
/// <remarks>
/// NOTE: Domain + System ErrorCode are temporarily unified.
/// If Domain is extracted as independent module in future,
/// split into DomainErrorCode / SystemErrorCode.
/// </remarks>
public sealed class ErrorCode : SmartEnum<ErrorCode>, IErrorCode
{
  /// <summary>
  /// 錯誤描述
  /// </summary>
  public string Description { get; }

  /// <summary>
  /// 錯誤來源（Domain、App、System、External）
  /// </summary>
  public ErrorSource Source { get; }

  /// <summary>
  /// 錯誤碼（4 位數字格式，例如 "7001"）
  /// </summary>
  public string Code => Value.ToString("D5");

  /// <summary>
  /// 初始化錯誤碼
  /// </summary>
  /// <param name="value">數值</param>
  /// <param name="name">名稱</param>
  /// <param name="desc">描述</param>
  /// <param name="source">錯誤來源</param>
  private ErrorCode(int value, string name, string desc, ErrorSource source)
      : base(value, name)
  {
    Description = desc;
    Source = source;
  }

  // === Domain ===
  public static readonly ErrorCode NameCannotBeEmpty =
      new(7001, nameof(NameCannotBeEmpty), "產品名稱不能為空", ErrorSource.Domain);
  public static readonly ErrorCode ProductNameTooLong =
      new(7002, nameof(ProductNameTooLong), "產品名稱過長", ErrorSource.Domain);
  public static readonly ErrorCode ProductNotFound =
      new(7002, nameof(ProductNotFound), "產品名稱不存在", ErrorSource.Domain);

  // === Application ===
  public static readonly ErrorCode ValidationFailed =
      new(8001, nameof(ValidationFailed), "資料驗證失敗", ErrorSource.App);

  // === System ===

  public static readonly ErrorCode DbError =
      new(9001, nameof(DbDeadlock), "DB Error", ErrorSource.App);
  public static readonly ErrorCode DbDeadlock =
      new(9002, nameof(DbDeadlock), "DB Deadlock", ErrorSource.App);

  public static readonly ErrorCode TokenAuthError =
      new(9401, nameof(TokenAuthError), "Token 驗證失敗", ErrorSource.App);
  public static readonly ErrorCode TokenCannotBeEmpty =
      new(9404, nameof(TokenCannotBeEmpty), "Token 不能為空", ErrorSource.App);
  public static readonly ErrorCode UserCannotBeEmptyError =
      new(9405, nameof(UserCannotBeEmptyError), "帳號 不能為空", ErrorSource.App);
  public static readonly ErrorCode PasswordCannotBeEmptyError =
      new(9406, nameof(PasswordCannotBeEmptyError), "密碼 不能為空", ErrorSource.App);

  public static readonly ErrorCode RedisTimeout =
      new(9100, nameof(RedisTimeout), "Redis Timeout", ErrorSource.App);

  public static readonly ErrorCode ApplicationLayerError =
    new(9999, nameof(ApplicationLayerError), "Application Layer Error", ErrorSource.App);


  public static readonly ErrorCode SystemError =
    new(99999, nameof(SystemError), "System Error", ErrorSource.System);
}

/// <summary>
/// 錯誤來源枚舉
/// 用於區分錯誤的來源層級
/// </summary>
public enum ErrorSource
{
  /// <summary>
  /// 領域層錯誤（業務規則違反）
  /// </summary>
  Domain,

  /// <summary>
  /// 應用層錯誤（應用層驗證失敗）
  /// </summary>
  App,

  /// <summary>
  /// 系統層錯誤（資料庫、基礎設施錯誤）
  /// </summary>
  System,

  /// <summary>
  /// 外部服務錯誤（第三方服務錯誤）
  /// </summary>
  External
}
