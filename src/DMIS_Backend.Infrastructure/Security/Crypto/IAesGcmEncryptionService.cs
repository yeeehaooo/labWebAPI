namespace DMIS_Backend.Infrastructure.Security.Crypto;

/// <summary>
/// AES-GCM 加密服務介面
/// 提供認證加密（Authenticated Encryption）
/// </summary>
public interface IAesGcmEncryptionService
{
  /// <summary>
  /// 使用 AES-GCM 加密資料
  /// </summary>
  /// <param name="plainText">明文</param>
  /// <param name="key">AES 金鑰（32 bytes）</param>
  /// <param name="aad">額外認證資料（可選）</param>
  /// <param name="nonce">Nonce/IV（12 bytes，可選，如果不提供則自動產生）</param>
  /// <returns>加密結果：IV(12 bytes) + Ciphertext + Tag(16 bytes)，Base64 編碼</returns>
  AesGcmEncryptionResult Encrypt(byte[] plainText, byte[] key, byte[]? aad = null, byte[]? nonce = null);

  /// <summary>
  /// 使用 AES-GCM 解密資料
  /// </summary>
  /// <param name="encryptedData">加密資料（Base64）</param>
  /// <param name="key">AES 金鑰（32 bytes）</param>
  /// <param name="aad">額外認證資料（可選）</param>
  /// <returns>解密後的明文</returns>
  byte[] Decrypt(string encryptedData, byte[] key, byte[]? aad = null);
}
