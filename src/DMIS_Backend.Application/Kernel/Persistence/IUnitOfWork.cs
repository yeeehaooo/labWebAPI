namespace DMIS_Backend.Application.Kernel.Persistence;

/// <summary>
/// Unit of Work 介面
/// 定義 UseCase 需要的「提交變更」能力
///
/// 核心原則：
/// - Application 層只關心「業務語意」：這個 UseCase 的變更需要被提交
/// - 不關心「技術細節」：如何開 DB transaction、如何 commit
/// - Infrastructure 層負責實作技術細節
/// </summary>
public interface IUnitOfWork
{
  /// <summary>
  /// 提交所有變更
  /// 代表「這個 UseCase 的變更需要被提交」的業務語意
  /// </summary>
  Task CommitAsync(CancellationToken cancellationToken = default);
}
