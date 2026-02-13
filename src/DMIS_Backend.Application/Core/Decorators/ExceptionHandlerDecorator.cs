using DMIS_Backend.Application.Core.Abstractions.Commands;
using DMIS_Backend.Application.Core.Results;
using DMIS_Backend.Domain.Kernel.Guards.Exceptions;
using Microsoft.Extensions.Logging;

namespace DMIS_Backend.Application.Core.Decorators;

/// <summary>
/// UseCase 執行裝飾器（Execution Pipeline Decorator） - 統一處理 UseCase 執行過程中的例外。<br />
/// Guard / Domain / App / Infrastructure 例外皆在此轉換為 Result。<br />
/// Middleware 僅處理 HTTP 層例外。
/// </summary>

public class ExceptionHandlerDecorator<TCommand, TResult>
    : IUseCaseCommandHandler<TCommand, TResult>
    where TCommand : IUseCaseCommand<TResult>
{
  private readonly IUseCaseCommandHandler<TCommand, TResult> _inner;
  private readonly ILogger<ExceptionHandlerDecorator<TCommand, TResult>> _logger;

  public ExceptionHandlerDecorator(
      IUseCaseCommandHandler<TCommand, TResult> inner,
      ILogger<ExceptionHandlerDecorator<TCommand, TResult>> logger)
  {
    _inner = inner;
    _logger = logger;
  }

  public async Task<Result<TResult>> HandleAsync(
      TCommand command,
      CancellationToken cancellationToken)
  {
    try
    {
      return await _inner.HandleAsync(command, cancellationToken);
    }
    catch (BusinessException businessEx)
    {
      _logger.LogWarning(businessEx,
          "Business failure: {Code} at {Member}:{Line} [{ConditionExpression}]",
          businessEx.ErrorCode.Value,
          businessEx.Context.Member,
          businessEx.Context.Line,
          businessEx.ConditionExpression);

      return ResultFactory.FromBusiness<TResult>(businessEx);
    }
  }
}
