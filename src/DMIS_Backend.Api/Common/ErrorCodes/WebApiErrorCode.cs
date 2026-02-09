using DMIS_Backend.Domain.Kernel.SmartEnums;

namespace DMIS_Backend.Api.Common.ErrorCodes;

/// <summary>
/// Web API Error Code - Web API 錯誤代碼 (9XX)
/// 系統例外 - 不可預期例外
/// </summary>
public sealed class WebApiErrorCode : SmartEnum<WebApiErrorCode>
{
  /// <summary>
  /// 內部伺服器錯誤
  /// </summary>
  public static readonly WebApiErrorCode InternalServerError = new(
      9999,
      "INTERNAL_SERVER_ERROR",
      "內部伺服器錯誤"
  );
  /// <summary>
  /// 資料庫連線錯誤
  /// </summary>
  public static readonly WebApiErrorCode DatabaseConnectionError = new(
      9001,
      "DATABASE_CONNECTION_ERROR",
      "資料庫連線錯誤"
  );

  /// <summary>
  /// 外部服務錯誤
  /// </summary>
  public static readonly WebApiErrorCode ExternalServiceError = new(
      9002,
      "EXTERNAL_SERVICE_ERROR",
      "外部服務錯誤"
  );

  /// <summary>
  /// 配置錯誤
  /// </summary>
  public static readonly WebApiErrorCode ConfigurationError = new(
      9003,
      "CONFIGURATION_ERROR",
      "配置錯誤"
  );

  /// <summary>
  /// 服務不可用
  /// </summary>
  public static readonly WebApiErrorCode ServiceUnavailable = new(
      9004,
      "SERVICE_UNAVAILABLE",
      "服務不可用"
  );

  /// <summary>
  /// 錯誤描述
  /// </summary>
  public string Description { get; }

  private WebApiErrorCode(int value, string name, string description)
      : base(value, name)
  {
    Description = description;
  }

  /// <summary>
  /// 取得所有 Web API 錯誤代碼
  /// </summary>
  public static IReadOnlyCollection<WebApiErrorCode> GetAllErrorCodes() => List;

  /// <summary>
  /// 轉換為字串（用於與現有 Result 類別整合）
  /// </summary>
  public string ToCodeString() => Name;

  /// <summary>
  /// 隱式轉換為字串
  /// </summary>
  public static implicit operator string(WebApiErrorCode errorCode) => errorCode.Description;
}
