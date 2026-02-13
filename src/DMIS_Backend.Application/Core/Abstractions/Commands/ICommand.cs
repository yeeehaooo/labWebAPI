namespace DMIS_Backend.Application.Core.Abstractions.Commands;

/// <summary>
/// API 層的 Command 介面
/// 定義 API DTO 轉換為 Application Command 的契約
/// </summary>
/// <typeparam name="TCommand">Application 層的 Command 類型</typeparam>
public interface ICommand<out TCommand> where TCommand : class
{
  /// <summary>
  /// 將 API DTO 轉換為 Application Command
  /// </summary>
  /// <returns>Application 層的 Command 實例</returns>
  TCommand ToCommand();
}
