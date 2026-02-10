using DMIS_Backend.Application.Kernel.Results;

namespace DMIS_Backend.Application.Kernel.Abstractions;

/// <summary>
/// Use Case Query Handler 介面
/// 定義查詢處理器的契約，負責處理業務查詢並返回結果
/// </summary>
/// <typeparam name="TQuery">查詢類型</typeparam>
/// <typeparam name="TResult">結果類型</typeparam>
public interface IUseCaseQueryHandler<TQuery, TResult>
    where TQuery : IUseCaseQuery<TResult>
{
  /// <summary>
  /// 處理查詢
  /// </summary>
  /// <param name="query">查詢</param>
  /// <param name="ct">取消令牌</param>
  /// <returns>處理結果</returns>
  Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken ct);
}
