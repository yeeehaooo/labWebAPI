namespace DMIS_Backend.Application.Core.Abstractions.Persistence;

/// <summary>
/// Unit of Work 介面
/// 定義 UseCase 需要的交易控制能力
///
/// 核心原則：
/// - Application 層可以手動控制交易範圍，避免整個 Handler 都包在交易中
/// - Handler 可以精確控制何時開啟交易、何時提交、何時回滾
/// - Infrastructure 層負責實作技術細節
///
/// 使用範例：
/// <code>
/// // 1. 先執行不需要交易的查詢
/// var existing = await _repo.GetByIdAsync(cmd.Id);
///
/// // 2. 手動開啟交易（只在需要時）
/// await _uow.BeginAsync();
///
/// try
/// {
///     // 3. 執行需要交易的寫入操作
///     await _repo.AddAsync(workOrder);
///     await _uow.CommitAsync();
/// }
/// catch
/// {
///     await _uow.RollbackAsync();
///     throw;
/// }
/// </code>
/// </summary>
public interface IUnitOfWork : IDisposable
{
  /// <summary>
  /// 開始交易
  /// Handler 必須在執行需要交易的寫入操作前手動呼叫此方法
  /// </summary>
  Task BeginAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// 提交所有變更
  /// 代表「這個 UseCase 的變更需要被提交」的業務語意
  /// 必須先呼叫 BeginAsync() 才能提交
  /// </summary>
  Task CommitAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// 回滾所有變更
  /// 當發生錯誤或業務邏輯判斷需要取消變更時使用
  /// 必須先呼叫 BeginAsync() 才能回滾
  /// </summary>
  Task RollbackAsync(CancellationToken cancellationToken = default);
}
