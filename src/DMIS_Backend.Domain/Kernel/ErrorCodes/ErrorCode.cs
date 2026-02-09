using DMIS_Backend.Domain.Kernel.SmartEnums;

namespace DMIS_Backend.Domain.Kernel.ErrorCodes;

public sealed class ErrorCode : SmartEnum<ErrorCode>, IErrorCode
{
  public string Description { get; }
  public ErrorSource Source { get; }
  public string Code => Value.ToString("D4");

  private ErrorCode(int value, string name, string desc, ErrorSource source)
      : base(value, name)
  {
    Description = desc;
    Source = source;
  }

  // === Domain (7xxx) ===
  public static readonly ErrorCode NameCannotBeEmpty =
      new(7001, nameof(NameCannotBeEmpty), "產品名稱不能為空", ErrorSource.Domain);
  // === Domain (7xxx) ===
  public static readonly ErrorCode ProductNameTooLong =
      new(7002, nameof(ProductNameTooLong), "產品名稱過長", ErrorSource.Domain);
  public static readonly ErrorCode ProductNotFound =
      new(7002, nameof(ProductNotFound), "產品名稱不存在", ErrorSource.Domain);
  // === App (8xxx) ===
  public static readonly ErrorCode ValidationFailed =
      new(8001, nameof(ValidationFailed), "資料驗證失敗", ErrorSource.App);

  // === System (9xxx) ===

  public static readonly ErrorCode DbError =
      new(3333, nameof(DbDeadlock), "DB Error", ErrorSource.System);
  public static readonly ErrorCode DbDeadlock =
      new(9001, nameof(DbDeadlock), "DB Deadlock", ErrorSource.System);

  public static readonly ErrorCode RedisTimeout =
      new(9002, nameof(RedisTimeout), "Redis Timeout", ErrorSource.System);

  public static readonly ErrorCode SystemError =
    new(9999, nameof(SystemError), "Application Error", ErrorSource.System);
}

public enum ErrorSource
{
  Domain,    // 業務規則
  App,       // 應用層驗證
  System,    // DB / Infra
  External   // 第三方服務
}
