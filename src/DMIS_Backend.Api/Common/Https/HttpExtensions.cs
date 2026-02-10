using System.Text;

namespace DMIS_Backend.Api.Common.Https;

/// <summary>
/// HTTP 相關的擴充方法
/// 用於讀取 Request/Response Body（不影響原始 Stream）
/// </summary>
public static class HttpExtensions
{
  /// <summary>
  /// 讀取請求 Body（不影響原始 Stream）
  /// </summary>
  /// <param name="request">HTTP 請求</param>
  /// <returns>Body 內容的 Base64 字串（用於日誌）</returns>
  public static async Task<string> ReadRequestBodyAsync(this HttpRequest request)
  {
    if (request == null)
    {
      throw new ArgumentNullException(nameof(request));
    }

    // 確保可以重複讀取
    request.EnableBuffering();

    // 保存原始位置
    var originalPosition = request.Body.Position;

    try
    {
      // 重置到開始位置
      request.Body.Position = 0;

      // 讀取內容
      using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
      var body = await reader.ReadToEndAsync();

      // 重置位置
      request.Body.Position = originalPosition;

      // 轉換為 Base64（用於日誌）
      var bodyBytes = Encoding.UTF8.GetBytes(body);
      return Convert.ToBase64String(bodyBytes);
    }
    finally
    {
      // 確保重置位置
      request.Body.Position = originalPosition;
    }
  }

  /// <summary>
  /// 讀取響應 Body
  /// </summary>
  /// <param name="responseBody">響應 Body Stream</param>
  /// <returns>Body 內容的 Base64 字串（用於日誌）</returns>
  public static async Task<string> ReadResponseBodyAsync(this Stream responseBody)
  {
    if (responseBody == null)
    {
      throw new ArgumentNullException(nameof(responseBody));
    }

    // 保存原始位置
    var originalPosition = responseBody.Position;

    try
    {
      // 重置到開始位置
      responseBody.Seek(0, SeekOrigin.Begin);

      // 讀取內容
      using var reader = new StreamReader(responseBody, Encoding.UTF8, leaveOpen: true);
      var body = await reader.ReadToEndAsync();

      // 重置位置
      responseBody.Seek(originalPosition, SeekOrigin.Begin);

      // 轉換為 Base64（用於日誌）
      var bodyBytes = Encoding.UTF8.GetBytes(body);
      return Convert.ToBase64String(bodyBytes);
    }
    finally
    {
      // 確保重置位置
      responseBody.Seek(originalPosition, SeekOrigin.Begin);
    }
  }
}
