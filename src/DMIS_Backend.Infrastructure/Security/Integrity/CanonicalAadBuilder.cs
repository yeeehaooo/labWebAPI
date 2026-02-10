using System.Security.Cryptography;
using System.Text;

namespace DMIS_Backend.Infrastructure.Security.Integrity;

/// <summary>
/// Canonical AAD 建構器
/// 用於建立標準化的額外認證資料（Additional Authenticated Data）
/// </summary>
public static class CanonicalAadBuilder
{
  /// <summary>
  /// 建構 Canonical AAD
  /// 格式：ts={timestamp}\nreqId={requestId}\nMETHOD={httpMethod}\npath={requestPath}\nnormalizedQuery={sortedQueryString}\nsha256(ciphertext)={hash}\nsha256(jwt)={hash}
  /// </summary>
  /// <param name="timestamp">Unix 時間戳記（秒）</param>
  /// <param name="requestId">請求唯一識別碼（UUID）</param>
  /// <param name="httpMethod">HTTP 方法（例如：POST, GET）</param>
  /// <param name="requestPath">請求路徑（例如：/api/product）</param>
  /// <param name="normalizedQuery">標準化的查詢字串（排序後的 key=value 對）</param>
  /// <param name="ciphertext">加密的資料（用於計算 hash）</param>
  /// <param name="jwt">JWT Token（用於計算 hash，可選）</param>
  /// <returns>Canonical AAD 的位元組陣列</returns>
  public static byte[] BuildCanonicalAAD(
    long timestamp,
    string requestId,
    string httpMethod,
    string requestPath,
    string? normalizedQuery,
    byte[] ciphertext,
    string? jwt = null
  )
  {
    if (string.IsNullOrWhiteSpace(requestId))
      throw new ArgumentException("RequestId cannot be null or empty", nameof(requestId));
    if (string.IsNullOrWhiteSpace(httpMethod))
      throw new ArgumentException("HttpMethod cannot be null or empty", nameof(httpMethod));
    if (string.IsNullOrWhiteSpace(requestPath))
      throw new ArgumentException("RequestPath cannot be null or empty", nameof(requestPath));
    if (ciphertext == null || ciphertext.Length == 0)
      throw new ArgumentException("Ciphertext cannot be null or empty", nameof(ciphertext));

    // 計算 sha256(ciphertext)
    var ciphertextHash = ComputeSha256(ciphertext);

    // 計算 sha256(jwt)（如果提供）
    var jwtHash = string.IsNullOrWhiteSpace(jwt) ? string.Empty : ComputeSha256(Encoding.UTF8.GetBytes(jwt));

    // 標準化查詢字串（如果為空則使用空字串）
    var query = normalizedQuery ?? string.Empty;

    // 建構 Canonical AAD 字串
    var aadString = $"ts={timestamp}\n" +
                    $"reqId={requestId}\n" +
                    $"METHOD={httpMethod}\n" +
                    $"path={requestPath}\n" +
                    $"normalizedQuery={query}\n" +
                    $"sha256(ciphertext)={ciphertextHash}\n" +
                    $"sha256(jwt)={jwtHash}";

    return Encoding.UTF8.GetBytes(aadString);
  }

  /// <summary>
  /// 計算 SHA256 Hash（小寫十六進位字串）
  /// </summary>
  private static string ComputeSha256(byte[] data)
  {
    using var sha256 = SHA256.Create();
    var hashBytes = sha256.ComputeHash(data);
    return Convert.ToHexString(hashBytes).ToLowerInvariant();
  }
}
