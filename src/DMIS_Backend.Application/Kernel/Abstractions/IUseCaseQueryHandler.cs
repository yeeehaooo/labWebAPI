using DMIS_Backend.Application.Kernel.Results;

namespace DMIS_Backend.Application.Kernel.Abstractions;

/// <summary>
/// Use Case Handler 介面
/// </summary>
public interface IUseCaseQueryHandler<TQuery, TResult>
    where TQuery : IUseCaseQuery<TResult>
{
  Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken ct);
}

