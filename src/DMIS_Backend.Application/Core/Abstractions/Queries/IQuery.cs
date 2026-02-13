namespace DMIS_Backend.Application.Core.Abstractions.Queries;

/// <summary>
/// API 層的 Query 介面
/// 定義 API DTO 轉換為 Application Query 的契約
/// </summary>
/// <typeparam name="TQuery">Application 層的 Query 類型</typeparam>
public interface IQuery<out TQuery> where TQuery : class
{
  /// <summary>
  /// 將 API DTO 轉換為 Application Query
  /// </summary>
  /// <returns>Application 層的 Query 實例</returns>
  TQuery ToQuery();
}
