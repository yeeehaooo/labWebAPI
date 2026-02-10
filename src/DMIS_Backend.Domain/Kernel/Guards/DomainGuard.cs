namespace DMIS_Backend.Domain.Kernel.Guards;

/// <summary>
/// Domain 層的 Guard
/// 用於領域不變條件驗證，只能拋出 DomainException
/// </summary>
public static class DomainGuard
{
  /// <summary>
  /// Domain 不變條件驗證（只能丟 DomainException）
  /// </summary>
  public static IDomainGuardClause Against { get; } = new DomainGuardClause();

  private sealed class DomainGuardClause : IDomainGuardClause { }
}

/// <summary>
/// Domain Guard 子句介面
/// 用於擴充驗證方法
/// </summary>
public interface IDomainGuardClause { }
