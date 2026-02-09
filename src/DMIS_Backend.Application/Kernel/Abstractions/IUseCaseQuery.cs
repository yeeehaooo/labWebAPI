namespace DMIS_Backend.Application.Kernel.Abstractions;

/// <summary>
/// Use Case Query 介面
/// 替代 IRequest&lt;TResponse&gt;，提供更清晰的語義
/// 繼承 IRequest&lt;TResponse&gt; 以保持向後相容
/// </summary>
/// <typeparam name="TResult">返回結果類型</typeparam>
public interface IUseCaseQuery<out TResult>
{
}
