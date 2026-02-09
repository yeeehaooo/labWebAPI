using DMIS_Backend.Application.Kernel.Persistence;

namespace DMIS_Backend.Infrastructure.Persistence;

/// <summary>
/// Unit of Work 的資料庫實作
/// 實作 Application 層定義的 IUnitOfWork 介面
///
/// 內部使用 IDbSession 管理交易（Infrastructure 內部技術細節）
///
/// 核心原則：
/// - Application 層只關心「提交變更」的業務語意
/// - Infrastructure 層負責處理所有技術細節：
///   1. 自動開始交易（如果還沒開始）
///   2. 提交交易
///   3. 錯誤時自動回滾（透過 IDisposable）
/// </summary>
public class DbUnitOfWork : IUnitOfWork, IDisposable
{
  private readonly IDbSession _session;
  private bool _transactionStarted = false;
  private bool _committed = false;
  private bool _disposed = false;

  public DbUnitOfWork(IDbSession session)
  {
    _session = session;
  }

  /// <summary>
  /// 提交所有變更
  ///
  /// 內部處理：
  /// 1. 如果交易還沒開始，自動開始交易
  /// 2. 提交交易
  ///
  /// Application 層不需要知道這些技術細節
  /// </summary>
  public async Task CommitAsync(CancellationToken cancellationToken = default)
  {
    if (_committed)
    {
      throw new InvalidOperationException("Unit of Work has already been committed.");
    }

    if (!_transactionStarted)
    {
      await _session.BeginAsync();
      _transactionStarted = true;
    }

    await _session.CommitAsync();
    _committed = true;
  }

  /// <summary>
  /// 釋放資源
  /// 如果交易已開始但還沒提交，自動回滾
  /// </summary>
  public void Dispose()
  {
    if (_disposed)
    {
      return;
    }

    // 如果交易已開始但還沒提交，自動回滾
    if (_transactionStarted && !_committed)
    {
      try
      {
        _session.RollbackAsync().GetAwaiter().GetResult();
      }
      catch
      {
        // 忽略回滾時的錯誤
      }
    }

    _disposed = true;
  }
}
