namespace DMIS_Backend.Api.Abstractions;

/// <summary>
/// Application Query 轉換為 API DTO 的介面
/// 提供反向轉換能力：Application Query → API DTO
/// </summary>
/// <typeparam name="TApiDto">API 層的 DTO 類型</typeparam>
public interface IQueryConvertible<out TApiDto>
    where TApiDto : class
{
    /// <summary>
    /// 將 Application Query 轉換為 API DTO
    /// </summary>
    /// <returns>API 層的 DTO 實例</returns>
    TApiDto ToApiDto();
}
