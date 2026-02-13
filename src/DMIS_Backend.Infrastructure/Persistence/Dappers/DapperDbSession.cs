using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DMIS_Backend.Infrastructure.Persistence.Dappers;

/// <summary>
/// Dapper 資料庫 Session 實作
/// 管理單一連線和交易，確保多個 Repository 共用同一個交易
/// </summary>
public class DapperDbSession : IDbSession
{
  private readonly IDbConnection _connection;
  private IDbTransaction? _transaction;
  private bool _disposed = false;

  /// <summary>
  /// 初始化 Dapper 資料庫 Session
  /// </summary>
  /// <param name="configuration">配置物件</param>
  public DapperDbSession(IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("DefaultConnection")
      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    _connection = new SqlConnection(connectionString);
  }

  /// <summary>
  /// 取得資料庫連線（所有 Repository 共用）
  /// </summary>
  public IDbConnection Connection => _connection;

  /// <summary>
  /// 取得資料庫交易（所有 Repository 共用）
  /// </summary>
  public IDbTransaction Transaction
  {
    get
    {
      if (_transaction == null)
      {
        throw new InvalidOperationException(
          "Transaction has not been started. Call BeginAsync() first."
        );
      }
      return _transaction;
    }
  }

  /// <summary>
  /// 開始交易
  /// 由 Application Handler 呼叫，確保交易邊界在應用層控制
  /// </summary>
  public async Task BeginAsync()
  {
    if (_connection.State != ConnectionState.Open)
    {
      await ((DbConnection)_connection).OpenAsync();
    }

    if (_transaction != null)
    {
      throw new InvalidOperationException("Transaction has already been started.");
    }

    _transaction = _connection.BeginTransaction();
  }

  /// <summary>
  /// 提交交易
  /// 由 Application Handler 呼叫，成功後提交所有變更
  /// </summary>
  public Task CommitAsync()
  {
    if (_transaction == null)
    {
      throw new InvalidOperationException("Transaction has not been started.");
    }

    _transaction.Commit();
    _transaction.Dispose();
    _transaction = null;

    return Task.CompletedTask;
  }

  /// <summary>
  /// 回滾交易
  /// 由 Application Handler 呼叫，發生錯誤時回滾所有變更
  /// </summary>
  public Task RollbackAsync()
  {
    if (_transaction == null)
    {
      // 如果交易還沒開始，就不需要回滾
      return Task.CompletedTask;
    }

    _transaction.Rollback();
    _transaction.Dispose();
    _transaction = null;

    return Task.CompletedTask;
  }

  /// <summary>
  /// 釋放資源
  /// </summary>
  public void Dispose()
  {
    if (_disposed)
    {
      return;
    }

    // 如果還有未提交的交易，先回滾
    if (_transaction != null)
    {
      try
      {
        _transaction.Rollback();
        _transaction.Dispose();
      }
      catch
      {
        // 忽略回滾時的錯誤（可能已經被提交或回滾）
      }
    }

    // 關閉連線
    if (_connection.State == ConnectionState.Open)
    {
      _connection.Close();
    }

    _connection.Dispose();
    _disposed = true;
  }
}
