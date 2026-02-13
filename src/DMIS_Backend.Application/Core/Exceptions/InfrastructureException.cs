using System.Runtime.CompilerServices;
using DMIS_Backend.Domain.Kernel.Guards;
using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Application.Core.Exceptions;

/// <summary>
/// 基礎設施抽象例外
/// </summary>
public abstract class InfrastructureException : Exception
{
  public ErrorCode ErrorCode { get; }

  public TraceContext Context { get; }

  protected InfrastructureException(
      ErrorCode errorCode,
      Exception? innerException = null,
      [CallerMemberName] string? member = null,
      [CallerFilePath] string? file = null,
      [CallerLineNumber] int line = 0)
      : base(errorCode.Message, innerException)
  {
    ErrorCode = errorCode;
    Context = new TraceContext(member, file, line);
  }
}


/// <summary>
/// 資料庫相關例外（MSSQL / EF / Dapper 等）
/// </summary>
public sealed class DatabaseException : InfrastructureException
{
  public DatabaseException(
      ErrorCode errorCode,
      Exception innerException)
      : base(errorCode, innerException)
  {
  }
}

/// <summary>
/// 快取相關例外（Redis / MemoryCache 等）
/// </summary>
public sealed class CacheException : InfrastructureException
{
  public CacheException(
      ErrorCode errorCode,
      Exception innerException)
      : base(errorCode, innerException)
  {
  }
}

/// <summary>
/// 外部服務例外（HTTP API / WebService 等）
/// </summary>
public sealed class ExternalServiceException : InfrastructureException
{
  /// <summary>
  /// 外部服務回應的 HTTP Status Code（如有）
  /// </summary>
  public int? HttpStatusCode { get; }

  public ExternalServiceException(
      ErrorCode errorCode,
      Exception innerException,
      int? httpStatusCode = null)
      : base(errorCode, innerException)
  {
    HttpStatusCode = httpStatusCode;
  }
}

/// <summary>
/// 安全相關基礎設施例外（加解密、JWT、簽章驗證等）
/// </summary>
public sealed class SecurityInfrastructureException : InfrastructureException
{
  public SecurityInfrastructureException(
      ErrorCode errorCode,
      Exception innerException)
      : base(errorCode, innerException)
  {
  }
}
