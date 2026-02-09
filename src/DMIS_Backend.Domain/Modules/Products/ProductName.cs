using DMIS_Backend.Domain.Kernel.Guards;

namespace DMIS_Backend.Domain.Modules.Products;

/// <summary>
/// 產品名稱值物件 - 封裝產品名稱驗證邏輯
/// </summary>
public record ProductName
{
  public string Value { get; init; }

  // 私有建構函式（用於 Dapper/ORM 映射）
  private ProductName()
  {
    Value = string.Empty;
  }

  public ProductName(string value)
  {
    DomainGuard.Against.Must(!string.IsNullOrWhiteSpace(value), ProductErrors.NameCannotBeEmpty);

    DomainGuard.Against.Must(
      value.Length <= 200,
      ProductErrors.NameTooLong, "長度太長DisplayName:產品名稱, MaxLength:200"
    );

    Value = value;
  }

  // 隱式轉換：方便與 string 互換
  public static implicit operator string(ProductName name) => name.Value;

  public static implicit operator ProductName(string value) => new(value);

  public override string ToString() => Value;
}
