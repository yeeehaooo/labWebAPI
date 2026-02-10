namespace DMIS_Backend.Domain.Kernel.SmartEnums;

/// <summary>
/// 用於指定 SmartEnum 值比較器的屬性
/// </summary>
/// <typeparam name="TValue">值類型</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SmartEnumComparerAttribute<TValue> : Attribute
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
  /// <summary>
  /// 值比較器
  /// </summary>
  public IEqualityComparer<TValue> Comparer { get; }

  /// <summary>
  /// 初始化 SmartEnumComparerAttribute
  /// </summary>
  /// <param name="comparer">值比較器</param>
  public SmartEnumComparerAttribute(IEqualityComparer<TValue> comparer)
  {
    Comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
  }
}
