namespace DMIS_Backend.Domain.Kernel.Primitives;
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
  Application,
  /// <summary>
  /// 驗證授權錯誤
  /// </summary>
  Auth,

  /// <summary>
  /// 系統層錯誤（資料庫、基礎設施錯誤）
  /// </summary>
  System,

  /// <summary>
  /// 外部服務錯誤（第三方服務錯誤）
  /// </summary>
  External
}
