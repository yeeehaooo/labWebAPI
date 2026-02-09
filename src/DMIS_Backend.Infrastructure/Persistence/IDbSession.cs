using System.Data;

namespace DMIS_Backend.Infrastructure.Persistence;

/// <summary>
/// 資料庫 Session 介面（Unit of Work 模式）
/// Infrastructure 內部技術抽象，用於管理 Dapper 的連線和交易
///
/// 核心原則：
/// - 連線由「外層建立」
/// - 交易由「外層控制」
/// - Repository 只「使用」傳入的 connection + transaction
/// </summary>
public interface IDbSession : IDisposable
{
  /// <summary>
  /// 取得資料庫連線（所有 Repository 共用同一個連線）
  /// </summary>
  IDbConnection Connection { get; }

  /// <summary>
  /// 取得資料庫交易（所有 Repository 共用同一個交易）
  /// </summary>
  IDbTransaction Transaction { get; }

  /// <summary>
  /// 開始交易（由 Application Handler 呼叫）
  /// </summary>
  Task BeginAsync();

  /// <summary>
  /// 提交交易（由 Application Handler 呼叫）
  /// </summary>
  Task CommitAsync();

  /// <summary>
  /// 回滾交易（由 Application Handler 呼叫）
  /// </summary>
  Task RollbackAsync();
}
