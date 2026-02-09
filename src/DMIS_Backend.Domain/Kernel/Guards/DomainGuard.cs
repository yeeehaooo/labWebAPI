namespace DMIS_Backend.Domain.Kernel.Guards;

public static class DomainGuard
{
  /// <summary>Domain 不變條件（只能丟 DomainException）</summary>
  public static IDomainGuardClause Against { get; } = new DomainGuardClause();

  private sealed class DomainGuardClause : IDomainGuardClause { }
}


public interface IDomainGuardClause { }
