namespace DMIS_Backend.Domain.Kernel;

/// <summary>
/// Aggregate Root 基類
/// 所有 Aggregate Root 都必須繼承此類別
/// </summary>
public abstract class AggregateRoot
{
  /// <summary>
  /// 業務識別碼（業務層面的唯一識別碼，通常是 Guid）
  /// </summary>
  public Guid AggregateId { get; protected set; }

  ///// <summary>
  ///// 建立時間
  ///// </summary>
  //public DateTime CreatedAt { get; protected set; }

  /// <summary>
  /// 受保護的建構函式，防止直接實例化
  /// </summary>
  protected AggregateRoot() { }

  /// <summary>
  /// 受保護的建構函式，用於建立新的 Aggregate
  /// </summary>
  protected AggregateRoot(Guid aggregateId)
  {
    AggregateId = aggregateId;
    //CreatedAt = DateTime.UtcNow;
  }
}
