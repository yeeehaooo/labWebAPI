# 加密服務說明文件

本專案提供兩種加密方式：

## 1. 對稱加密 (AES256)

### 用途
- 大量資料加密
- 資料庫欄位加密
- 內部系統間資料傳輸

### 特點
- 加密/解密速度快
- 使用同一把金鑰
- 適合加密大量資料

### 使用方式

```csharp
public class MyService
{
    private readonly IEncryptionProvider _encryptionProvider;

    public MyService(IEncryptionProvider encryptionProvider)
    {
        _encryptionProvider = encryptionProvider;
    }

    public void EncryptData()
    {
        var plainText = "敏感資料";
        var encrypted = _encryptionProvider.Encrypt(plainText);
        var decrypted = _encryptionProvider.Decrypt(encrypted);
    }
}
```

### 設定檔 (appsettings.json)

```json
{
  "Encryption": {
    "Key": "your-32-byte-key-here-base64-encoded",
    "IV": "your-16-byte-iv-here-base64-encoded"
  }
}
```

### 產生金鑰

```csharp
var (key, iv) = EncryptionKeyGenerator.GenerateRandomKeys();
Console.WriteLine($"Key: {key}");
Console.WriteLine($"IV: {iv}");
```

---

## 2. 非對稱加密 (RSA)

### 用途
- 金鑰交換
- 數位簽章
- 小量敏感資料加密
- API 金鑰加密傳輸

### 特點
- 公鑰加密，私鑰解密
- 支援數位簽章和驗證
- 安全性高，但速度較慢
- 有資料長度限制（建議用於加密小資料或金鑰）

### 使用方式

#### 基本加密/解密

```csharp
public class MyService
{
    private readonly IRsaEncryptionProvider _rsaProvider;

    public MyService(IRsaEncryptionProvider rsaProvider)
    {
        _rsaProvider = rsaProvider;
    }

    public void EncryptData()
    {
        var plainText = "敏感資料";

        // 使用公鑰加密
        var encrypted = _rsaProvider.Encrypt(plainText);

        // 使用私鑰解密
        var decrypted = _rsaProvider.Decrypt(encrypted);
    }
}
```

#### 數位簽章和驗證

```csharp
public void SignAndVerify()
{
    var data = "重要資料";

    // 使用私鑰簽章
    var signature = _rsaProvider.SignData(data);

    // 使用公鑰驗證
    var isValid = _rsaProvider.VerifySignature(data, signature);

    if (isValid)
    {
        Console.WriteLine("簽章驗證通過");
    }
}
```

### 設定檔 (appsettings.json)

```json
{
  "RsaEncryption": {
    "PublicKey": "base64-encoded-public-key",
    "PrivateKey": "base64-encoded-private-key"
  }
}
```

### 產生 RSA 金鑰對

```csharp
// 產生 2048 bits 金鑰對（推薦用於一般應用）
var (publicKey, privateKey) = RsaKeyGenerator.GenerateKeyPair2048();

// 或產生 4096 bits 金鑰對（推薦用於高安全性需求）
var (publicKey4096, privateKey4096) = RsaKeyGenerator.GenerateKeyPair4096();

Console.WriteLine($"Public Key: {publicKey}");
Console.WriteLine($"Private Key: {privateKey}");
```

### 注意事項

1. **資料長度限制**：
   - RSA 2048 bits：最多可加密 245 bytes
   - RSA 4096 bits：最多可加密 501 bytes
   - 超過限制的資料會自動分段加密/解密

2. **混合加密模式**：
   - 對於大量資料，建議使用混合加密：
     - 使用 RSA 加密 AES 金鑰
     - 使用 AES 加密實際資料
     - 將加密的 AES 金鑰和加密的資料一起傳輸

3. **私鑰安全**：
   - 私鑰必須妥善保管，不可洩露
   - 生產環境建議使用 Azure Key Vault 或類似服務

---

## 混合加密範例（推薦用於大量資料）

結合 RSA 和 AES 的優點：

```csharp
public class HybridEncryptionService
{
    private readonly IEncryptionProvider _aesProvider;
    private readonly IRsaEncryptionProvider _rsaProvider;

    public HybridEncryptionService(
        IEncryptionProvider aesProvider,
        IRsaEncryptionProvider rsaProvider)
    {
        _aesProvider = aesProvider;
        _rsaProvider = rsaProvider;
    }

    public (string EncryptedData, string EncryptedKey) EncryptLargeData(string plainText)
    {
        // 1. 產生臨時 AES 金鑰
        var (aesKey, aesIv) = EncryptionKeyGenerator.GenerateRandomKeys();

        // 2. 使用 AES 加密資料
        var encryptedData = _aesProvider.Encrypt(plainText);

        // 3. 使用 RSA 加密 AES 金鑰
        var encryptedKey = _rsaProvider.Encrypt($"{aesKey}:{aesIv}");

        return (encryptedData, encryptedKey);
    }

    public string DecryptLargeData(string encryptedData, string encryptedKey)
    {
        // 1. 使用 RSA 解密 AES 金鑰
        var keyAndIv = _rsaProvider.Decrypt(encryptedKey);
        var parts = keyAndIv.Split(':');
        var aesKey = parts[0];
        var aesIv = parts[1];

        // 2. 使用 AES 解密資料
        // 注意：這裡需要建立新的 AES Provider 使用解密後的金鑰
        // 實際實作時需要根據您的架構調整

        return _aesProvider.Decrypt(encryptedData);
    }
}
```

---

## 何時使用哪種加密？

| 場景 | 推薦加密方式 |
|------|------------|
| 資料庫欄位加密 | AES256 |
| 大量資料加密 | AES256 |
| 金鑰交換 | RSA |
| 數位簽章 | RSA |
| API 金鑰傳輸 | RSA |
| 小量敏感資料 | RSA |
| 大量敏感資料 | 混合加密（RSA + AES） |

---

## 安全性建議

1. **金鑰管理**：
   - 使用環境變數或 Azure Key Vault 儲存金鑰
   - 不要在程式碼中硬編碼金鑰
   - 定期輪換金鑰

2. **傳輸安全**：
   - 始終使用 HTTPS 傳輸加密資料
   - 不要在 URL 參數中傳遞敏感資料

3. **儲存安全**：
   - 加密後的資料仍需要適當的存取控制
   - 定期備份金鑰（安全地）

4. **效能考量**：
   - RSA 加密較慢，避免在高頻 API 中使用
   - 大量資料優先使用 AES
