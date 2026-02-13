namespace DMIS_Backend.Domain.Kernel;

/// <summary>
/// 泛型 Repository 介面
/// 只接受 AggregateRoot 作為泛型參數，符合 DDD 原則
/// </summary>
/// <typeparam name="TAggregate">Aggregate Root 類型，必須繼承 AggregateRoot</typeparam>
public interface IRepository<TAggregate>
    where TAggregate : AggregateRoot
{
  /// <summary>
  /// 根據 Id 取得 Aggregate
  /// </summary>
  Task<TAggregate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

  /// <summary>
  /// 根據 AggregateId 取得 Aggregate
  /// </summary>
  Task<TAggregate?> GetByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken = default);

  /// <summary>
  /// 取得所有 Aggregate
  /// </summary>
  Task<IEnumerable<TAggregate>> GetAllAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// 分頁取得 Aggregate
  /// </summary>
  Task<IEnumerable<TAggregate>> GetByPageAsync(
    int pageNumber,
    int pageSize,
    CancellationToken cancellationToken = default
  );

  /// <summary>
  /// 新增 Aggregate
  /// </summary>
  Task AddAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

  /// <summary>
  /// 更新 Aggregate
  /// </summary>
  Task UpdateAsync(TAggregate aggregate, CancellationToken cancellationToken = default);

  /// <summary>
  /// 刪除 Aggregate
  /// </summary>
  Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
