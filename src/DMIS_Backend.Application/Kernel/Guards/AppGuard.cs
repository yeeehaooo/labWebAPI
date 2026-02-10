namespace DMIS_Backend.Application.Kernel.Guards;

/// <summary>
/// Application 層的 Guard
/// 用於應用層驗證，只能拋出 AppException
/// </summary>
public static class AppGuard
{
  /// <summary>
  /// Application / UseCase 驗證（只能丟 AppException）
  /// </summary>
  public static IAppGuardClause Against { get; } = new ApplicationGuardClause();

  private sealed class ApplicationGuardClause : IAppGuardClause { }
}

/// <summary>
/// Application Guard 子句介面
/// 用於擴充驗證方法
/// </summary>
public interface IAppGuardClause { }
