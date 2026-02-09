namespace DMIS_Backend.Domain.Kernel.SmartEnums;

/// <summary>
/// SmartEnum When/Then 模式的執行器
/// </summary>
/// <typeparam name="TEnum">SmartEnum 類型</typeparam>
/// <typeparam name="TValue">值類型</typeparam>
public class SmartEnumThen<TEnum, TValue>
    where TEnum : SmartEnum<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
  private readonly bool _condition;
  private readonly bool _executed;
  private readonly SmartEnum<TEnum, TValue> _smartEnum;

  internal SmartEnumThen(bool condition, bool executed, SmartEnum<TEnum, TValue> smartEnum)
  {
    _condition = condition;
    _executed = executed;
    _smartEnum = smartEnum;
  }

  /// <summary>
  /// 當條件為 true 時執行指定的動作
  /// </summary>
  /// <param name="action">要執行的動作</param>
  /// <returns>執行器實例，可繼續鏈式調用</returns>
  public SmartEnumThen<TEnum, TValue> Then(Action<SmartEnum<TEnum, TValue>> action)
  {
    if (_condition && !_executed)
    {
      action(_smartEnum);
      return new SmartEnumThen<TEnum, TValue>(_condition, true, _smartEnum);
    }

    return new SmartEnumThen<TEnum, TValue>(_condition, _executed, _smartEnum);
  }

  /// <summary>
  /// 當條件為 false 時執行指定的動作
  /// </summary>
  /// <param name="action">要執行的動作</param>
  /// <returns>執行器實例</returns>
  public SmartEnumThen<TEnum, TValue> Else(Action<SmartEnum<TEnum, TValue>> action)
  {
    if (!_condition && !_executed)
    {
      action(_smartEnum);
      return new SmartEnumThen<TEnum, TValue>(_condition, true, _smartEnum);
    }

    return new SmartEnumThen<TEnum, TValue>(_condition, _executed, _smartEnum);
  }
}
