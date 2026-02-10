using DMIS_Backend.Domain.Kernel.SmartEnums;

namespace DMIS_Backend.Application.Kernel.Workflows.IdentifierCodes;

/// <summary>
/// 命名空間代碼基類
/// 提供代碼和描述的基礎功能，用於定義模組、操作類型、功能範圍等識別碼
/// </summary>
/// <typeparam name="T">具體的識別碼類型</typeparam>
public abstract class NamespaceCode<T> : SmartEnum<T>
    where T : SmartEnum<T>
{
  /// <summary>
  /// 描述資訊
  /// </summary>
  public string Description { get; }
  /// <summary>
  /// 代碼（預設為名稱）
  /// </summary>
  public string Code => Name;

  /// <summary>
  /// 初始化命名空間代碼
  /// </summary>
  /// <param name="value">數值</param>
  /// <param name="name">名稱</param>
  /// <param name="description">描述（可選）</param>
  protected NamespaceCode(int value, string name, string description = null)
      : base(value, name)
  {
    Description = description;
  }
}



