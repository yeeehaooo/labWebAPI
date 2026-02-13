using DMIS_Backend.Application.Core.Abstractions.Persistence;

namespace DMIS_Backend.Infrastructure.Persistence.Dappers;

/// <summary>
/// Unit of Work 的 Dapper 實作
/// 實作 Application 層定義的 IUnitOfWork 介面
///
/// 內部使用 IDbSession 管理交易（Infrastructure 內部技術細節）
///
/// 核心原則：
/// - Handler 必須手動呼叫 BeginAsync() 來開始交易
/// - Handler 可以精確控制交易範圍，避免整個 Handler 都包在交易中
/// - Infrastructure 層負責處理技術細節：
///   1. 管理交易狀態
///   2. 提交交易
///   3. 錯誤時自動回滾（透過 IDisposable）
/// </summary>
public class DapperUnitOfWork : IUnitOfWork, IDisposable
{
  private readonly IDbSession _session;
  private bool _transactionStarted = false;
  private bool _committed = false;
  private bool _rolledBack = false;
  private bool _disposed = false;

  /// <summary>
  /// 初始化 Unit of Work
  /// </summary>
  /// <param name="session">資料庫 Session</param>
  public DapperUnitOfWork(IDbSession session)
  {
    _session = session;
  }

  /// <summary>
  /// 開始交易
  /// Handler 必須在執行需要交易的寫入操作前手動呼叫此方法
  /// </summary>
  public async Task BeginAsync(CancellationToken cancellationToken = default)
  {
    if (_disposed)
    {
      throw new ObjectDisposedException(nameof(DapperUnitOfWork));
    }

    if (_transactionStarted)
    {
      throw new InvalidOperationException("Transaction has already been started.");
    }

    if (_committed)
    {
      throw new InvalidOperationException("Unit of Work has already been committed.");
    }

    if (_rolledBack)
    {
      throw new InvalidOperationException("Unit of Work has already been rolled back.");
    }

    await _session.BeginAsync();
    _transactionStarted = true;
  }

  /// <summary>
  /// 提交所有變更
  ///
  /// 內部處理：
  /// 1. 檢查交易是否已開始（必須先呼叫 BeginAsync）
  /// 2. 提交交易
  ///
  /// Application 層必須先呼叫 BeginAsync() 才能提交
  /// </summary>
  public async Task CommitAsync(CancellationToken cancellationToken = default)
  {
    if (_disposed)
    {
      throw new ObjectDisposedException(nameof(DapperUnitOfWork));
    }

    if (_committed)
    {
      throw new InvalidOperationException("Unit of Work has already been committed.");
    }

    if (_rolledBack)
    {
      throw new InvalidOperationException("Cannot commit after rollback.");
    }

    if (!_transactionStarted)
    {
      throw new InvalidOperationException(
        "Transaction has not been started. Call BeginAsync() before CommitAsync()."
      );
    }

    await _session.CommitAsync();
    _committed = true;
  }

  /// <summary>
  /// 回滾所有變更
  ///
  /// 當發生錯誤或業務邏輯判斷需要取消變更時使用
  /// 必須先呼叫 BeginAsync() 才能回滾
  /// </summary>
  public async Task RollbackAsync(CancellationToken cancellationToken = default)
  {
    if (_disposed)
    {
      throw new ObjectDisposedException(nameof(DapperUnitOfWork));
    }

    if (_committed)
    {
      throw new InvalidOperationException("Cannot rollback after commit.");
    }

    if (_rolledBack)
    {
      // 已經回滾過，不需要再次回滾
      return;
    }

    if (!_transactionStarted)
    {
      // 如果交易還沒開始，就不需要回滾
      return;
    }

    await _session.RollbackAsync();
    _rolledBack = true;
  }

  /// <summary>
  /// 釋放資源
  /// 如果交易已開始但還沒提交或回滾，自動回滾
  /// </summary>
  public void Dispose()
  {
    if (_disposed)
    {
      return;
    }

    // 如果交易已開始但還沒提交或回滾，自動回滾
    if (_transactionStarted && !_committed && !_rolledBack)
    {
      try
      {
        _session.RollbackAsync().GetAwaiter().GetResult();
      }
      catch
      {
        // 忽略回滾時的錯誤（可能已經被提交或回滾）
      }
    }

    _disposed = true;
  }
}
