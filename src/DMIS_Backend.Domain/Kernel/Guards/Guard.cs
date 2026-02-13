namespace DMIS_Backend.Domain.Kernel.Guards;

/// <summary>
/// 用於應用層驗證，只能拋出 AppException
/// </summary>
public static class Guard
{
  public static IDomainGuardClause Domain { get; } = new DomainGuard();
  public static IApplicationGuardClause Application { get; } = new ApplicationGuard();


  private sealed class DomainGuard : IDomainGuardClause { }

  private sealed class ApplicationGuard : IApplicationGuardClause { }
}



/// <summary>
/// Domain Guard 子句介面
/// 用於擴充驗證方法
/// </summary>
public interface IDomainGuardClause { }

/// <summary>
/// Application Guard 子句介面
/// 用於擴充驗證方法
/// </summary>
public interface IApplicationGuardClause { }

