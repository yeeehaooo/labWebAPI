namespace DMIS_Backend.Application.Kernel.Guards;

public static class AppGuard
{
  /// <summary>
  /// Application / UseCase 驗證（只能丟 AppException）
  /// </summary>
  public static IAppGuardClause Against { get; } = new ApplicationGuardClause();

  private sealed class ApplicationGuardClause : IAppGuardClause { }
}

public interface IAppGuardClause { }
