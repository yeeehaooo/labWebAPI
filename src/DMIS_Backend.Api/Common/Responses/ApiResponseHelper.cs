using DMIS_Backend.Application.Core.Results;
using DMIS_Backend.Application.Core.Workflows;
using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Api.Common.Responses;

/// <summary>
/// Result 轉換為 APIResponse 的映射器
/// 分離業務層（Result）和表現層（APIResponse）
/// </summary>
public static class ApiResponseHelper
{
  public static APIResponse<T> ToApiResponse<T>(
        this Result<T> result)
  {
    var finalCode = Workflow.Build(result.Error);

    return new APIResponse<T>(
        finalCode,
        result.Message,
        result.IsSuccess ? result.Data : default);
  }

  public static APIResponse ToApiResponse(
      this Result result)
  {
    var finalCode = Workflow.Build(result.Error);

    return new APIResponse(
        finalCode,
        result.Message);
  }


  public static APIResponse Success(string message = null)
        => new(SystemCode.Success.Value, message ?? SystemCode.Success.Message) { Code = Workflow.Build(SystemCode.Success) };
  public static APIResponse<T> Success<T>(T data, string message = null)
        => new(SystemCode.Success.Value, message ?? SystemCode.Success.Message, data) { Code = Workflow.Build(SystemCode.Success) };
  public static APIResponse<T> Failure<T>(ErrorCode error, string? message = null, object? meta = null)
    => new(error.Value, message ?? error.Message, default, meta) { Code = Workflow.Build(error) };
}

