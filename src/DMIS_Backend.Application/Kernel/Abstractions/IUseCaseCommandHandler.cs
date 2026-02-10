using DMIS_Backend.Application.Kernel.Results;

namespace DMIS_Backend.Application.Kernel.Abstractions;

/// <summary>
/// Use Case Command Handler 介面
/// 定義命令處理器的契約，負責處理業務命令並返回結果
/// </summary>
/// <typeparam name="TCommand">命令類型</typeparam>
/// <typeparam name="TResult">結果類型</typeparam>
public interface IUseCaseCommandHandler<TCommand, TResult>
    where TCommand : IUseCaseCommand<TResult>
{
  /// <summary>
  /// 處理命令
  /// </summary>
  /// <param name="command">命令</param>
  /// <param name="ct">取消令牌</param>
  /// <returns>處理結果</returns>
  Task<Result<TResult>> HandleAsync(TCommand command, CancellationToken ct);
}
