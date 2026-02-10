using DMIS_Backend.Application.Kernel.Abstractions;
using DMIS_Backend.Application.Kernel.Guards;
using DMIS_Backend.Domain.Kernel.ErrorCodes;
using DMIS_Backend.Domain.Kernel.Guards.Exceptions;

namespace DMIS_Backend.Application.Kernel.Results;

/// <summary>
/// Result Pattern - 結果模式
/// 用於統一處理操作的成功或失敗狀態，避免使用異常進行控制流程
/// </summary>
public class Result<T> : IHandlerResult
{
  private readonly T? _data;

  /// <summary>
  /// 初始化 Result
  /// </summary>
  /// <param name="success">是否成功</param>
  /// <param name="code">錯誤碼</param>
  /// <param name="message">訊息</param>
  /// <param name="data">資料</param>
  /// <param name="error">錯誤詳細資訊</param>
  protected Result(bool success, string? code, string message, T? data, ErrorDetail? error)
  {
    IsSuccess = success;
    Code = code;
    _data = data;
    Message = message;
    Error = error;
  }
  /// <summary>
  /// 非泛型視角的資料（給 Filter 使用）
  /// </summary>
  public object? Data => IsSuccess ? _data : null;

  /// <summary>
  /// 強型別的資料值（成功時才有值）
  /// </summary>
  public T? Value => IsSuccess ? _data! : default;

  /// <summary>
  /// 是否成功
  /// </summary>
  public bool IsSuccess { get; }

  /// <summary>
  /// 是否失敗
  /// </summary>
  public bool IsFailure => !IsSuccess;

  /// <summary>
  /// 錯誤碼
  /// </summary>
  public string? Code { get; }

  /// <summary>
  /// 訊息
  /// </summary>
  public string? Message { get; }

  /// <summary>
  /// 錯誤詳細資訊
  /// </summary>
  public ErrorDetail? Error { get; }

  ///// <summary>
  ///// 完整流程錯誤碼（例如 "DES-1-DES020-7001"）
  ///// 此欄位由 API 層組合設定，用於向後相容
  ///// </summary>
  //public string? WorkflowCode { get; internal set; }

  /// <summary>
  /// 建立成功結果（無資料）
  /// </summary>
  /// <returns>成功結果</returns>
  public static Result<T> Success() => new(true, "0000", string.Empty, default, null);

  /// <summary>
  /// 建立成功結果（含資料）
  /// </summary>
  /// <param name="data">資料</param>
  /// <returns>成功結果</returns>
  public static Result<T> Success(T data) => new(true, "0000", string.Empty, data, null);
  /// <summary>
  /// 建立失敗結果
  /// </summary>
  /// <param name="code">錯誤碼</param>
  /// <param name="message">錯誤訊息</param>
  /// <param name="error">錯誤詳細資訊</param>
  /// <returns>失敗結果</returns>
  public static Result<T> Failure(string code, string message, ErrorDetail error)
    => new(false, code, message, default, error);

  //public static Result Failure(ErrorCode errorCode)
  //{
  //  var error = new ErrorDetail(
  //      errorCode: errorCode
  //  );

  //  return new Result(false, errorCode.Code, errorCode.Description, error);
  //}
  /// <summary>
  /// 從 GuardException 建立失敗結果
  /// </summary>
  /// <param name="ex">Guard 例外</param>
  /// <returns>失敗結果</returns>
  public static Result<T> Failure(GuardException<IErrorCode> ex)
  {
    var error = new ErrorDetail(
        errorCode: ex.ErrorCode,
        guardTrace: ex.Context.DiagnosticInfo
    );

    return new Result<T>(
        success: false,
        code: ex.ErrorCode.Code,
        data: default,
        message: ex.DisplayMessage,
        error: error
    );
  }
  /// <summary>
  /// 從錯誤碼和例外建立失敗結果
  /// </summary>
  /// <param name="errorCode">錯誤碼</param>
  /// <param name="ex">例外</param>
  /// <returns>失敗結果</returns>
  public static Result<T> Failure(IErrorCode errorCode, Exception ex)
  {
    var error = new ErrorDetail(
        errorCode: errorCode,
        guardTrace: null,
        stackTrace: null //太多 ex.StackTrace
    );

    return new Result<T>(
        success: false,
        code: errorCode.Code,
        data: default,
        message: ex.Message,
        error: error
    );
  }
  /// <summary>
  /// 隱式轉換：從 ErrorDetail 轉換為 Result
  /// </summary>
  public static implicit operator Result<T>(GuardException<IErrorCode> exception) => Failure(exception);


  /// <summary>
  /// 映射結果資料到另一種類型
  /// </summary>
  /// <typeparam name="TOut">目標類型</typeparam>
  /// <param name="mapper">映射函數</param>
  /// <returns>映射後的結果</returns>
  public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
  {
    if (IsFailure)
    {
      return Result<TOut>.Failure(Code!, Message, Error!);
    }

    var newValue = mapper(_data!);
    return Result<TOut>.Success(newValue);
  }
}
