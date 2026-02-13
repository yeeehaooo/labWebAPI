namespace DMIS_Backend.Infrastructure.Security.Cryptography.Asymmetric;

/// <summary>
/// RSA 非對稱加密/解密提供者介面
/// 用於需要非對稱加密的場景（如：金鑰交換、數位簽章、敏感資料加密）
/// </summary>
public interface IRsaEncryptionProvider
{
  /// <summary>
  /// 使用公鑰加密資料
  /// </summary>
  /// <param name="plainText">明文</param>
  /// <returns>Base64 編碼的加密字串</returns>
  string Encrypt(string plainText);

  /// <summary>
  /// 使用私鑰解密資料
  /// </summary>
  /// <param name="cipherText">Base64 編碼的加密字串</param>
  /// <returns>解密後的明文</returns>
  string Decrypt(string cipherText);

  /// <summary>
  /// 使用私鑰簽章資料
  /// </summary>
  /// <param name="data">要簽章的資料</param>
  /// <returns>Base64 編碼的簽章</returns>
  string SignData(string data);

  /// <summary>
  /// 使用公鑰驗證簽章
  /// </summary>
  /// <param name="data">原始資料</param>
  /// <param name="signature">Base64 編碼的簽章</param>
  /// <returns>驗證是否通過</returns>
  bool VerifySignature(string data, string signature);

  /// <summary>
  /// 取得公鑰（Base64 編碼）
  /// </summary>
  string GetPublicKey();

  /// <summary>
  /// 取得私鑰（Base64 編碼，僅用於內部解密和簽章）
  /// </summary>
  string GetPrivateKey();
}
