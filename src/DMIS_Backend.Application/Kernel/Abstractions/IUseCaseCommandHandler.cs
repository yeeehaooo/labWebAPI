using DMIS_Backend.Application.Kernel.Results;

namespace DMIS_Backend.Application.Kernel.Abstractions;

/// <summary>
/// Use Case Handler 介面
/// </summary>
public interface IUseCaseCommandHandler<TCommand, TResult>
    where TCommand : IUseCaseCommand<TResult>
{
  Task<Result<TResult>> HandleAsync(TCommand command, CancellationToken ct);
}

