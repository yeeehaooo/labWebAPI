namespace DMIS_Backend.Infrastructure.Security.Integrity;

/// <summary>
/// HMAC 簽名服務介面
/// 用於驗證請求的完整性
/// </summary>
public interface IHmacSignatureService
{
  /// <summary>
  /// 產生 HMAC 簽名
  /// </summary>
  /// <param name="key">HMAC 金鑰（AES Key）</param>
  /// <param name="data">要簽名的資料</param>
  /// <returns>HMAC 簽名（Base64）</returns>
  string GenerateSignature(byte[] key, string data);

  /// <summary>
  /// 驗證 HMAC 簽名
  /// </summary>
  /// <param name="key">HMAC 金鑰（AES Key）</param>
  /// <param name="data">原始資料</param>
  /// <param name="signature">要驗證的簽名（Base64）</param>
  /// <returns>驗證是否通過</returns>
  bool VerifySignature(byte[] key, string data, string signature);
}
