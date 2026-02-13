namespace DMIS_Backend.Infrastructure.Security.Cryptography.Symmetric;

/// <summary>
/// AES-GCM 加密結果
/// </summary>
public sealed record AesGcmEncryptionResult
{
  /// <summary>
  /// 初始化向量（IV/Nonce，12 bytes）
  /// </summary>
  public byte[] Nonce { get; init; } = Array.Empty<byte>();

  /// <summary>
  /// 加密後的資料
  /// </summary>
  public byte[] Ciphertext { get; init; } = Array.Empty<byte>();

  /// <summary>
  /// 認證標籤（Tag，16 bytes）
  /// </summary>
  public byte[] Tag { get; init; } = Array.Empty<byte>();

  /// <summary>
  /// 完整加密資料（Base64）：IV + Ciphertext + Tag
  /// </summary>
  public string ToBase64String()
  {
    var combined = new byte[Nonce.Length + Ciphertext.Length + Tag.Length];
    Array.Copy(Nonce, 0, combined, 0, Nonce.Length);
    Array.Copy(Ciphertext, 0, combined, Nonce.Length, Ciphertext.Length);
    Array.Copy(Tag, 0, combined, Nonce.Length + Ciphertext.Length, Tag.Length);
    return Convert.ToBase64String(combined);
  }

  /// <summary>
  /// 從 Base64 字串解析
  /// </summary>
  public static AesGcmEncryptionResult FromBase64String(string base64String, int nonceLength = 12, int tagLength = 16)
  {
    var combined = Convert.FromBase64String(base64String);
    var ciphertextLength = combined.Length - nonceLength - tagLength;

    if (ciphertextLength < 0)
    {
      throw new ArgumentException("Invalid encrypted data format. Data is too short.", nameof(base64String));
    }

    var nonce = new byte[nonceLength];
    var ciphertext = new byte[ciphertextLength];
    var tag = new byte[tagLength];

    Array.Copy(combined, 0, nonce, 0, nonceLength);
    Array.Copy(combined, nonceLength, ciphertext, 0, ciphertextLength);
    Array.Copy(combined, nonceLength + ciphertextLength, tag, 0, tagLength);

    return new AesGcmEncryptionResult
    {
      Nonce = nonce,
      Ciphertext = ciphertext,
      Tag = tag
    };
  }
}
