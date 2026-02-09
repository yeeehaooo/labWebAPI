namespace DMIS_Backend.Api.Abstractions;

/// <summary>
/// Application Command 轉換為 API DTO 的介面
/// 提供反向轉換能力：Application Command → API DTO
/// </summary>
/// <typeparam name="TApiDto">API 層的 DTO 類型</typeparam>
public interface ICommandConvertible<out TApiDto>
    where TApiDto : class
{
    /// <summary>
    /// 將 Application Command 轉換為 API DTO
    /// </summary>
    /// <returns>API 層的 DTO 實例</returns>
    TApiDto ToApiDto();
}
