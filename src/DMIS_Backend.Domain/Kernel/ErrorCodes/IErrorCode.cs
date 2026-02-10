namespace DMIS_Backend.Domain.Kernel.ErrorCodes;

public interface IErrorCode
{
  /// <summary>
  /// 0000(成功) ~ 9999(失敗)
  /// </summary>
  string Code { get; }

  /// <summary>
  /// 預設訊息
  /// </summary>
  string Description { get; }

  /// <summary>
  /// 錯誤來源類型
  /// </summary>
  ErrorSource Source { get; }
}


