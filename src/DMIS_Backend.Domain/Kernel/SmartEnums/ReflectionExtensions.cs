using System.Reflection;

namespace DMIS_Backend.Domain.Kernel.SmartEnums;

/// <summary>
/// Reflection 擴展方法
/// </summary>
internal static class ReflectionExtensions
{
  /// <summary>
  /// 取得指定類型的所有欄位，並過濾為指定類型的實例
  /// </summary>
  /// <typeparam name="T">目標類型</typeparam>
  /// <param name="type">要搜尋的類型</param>
  /// <returns>符合條件的欄位值列表</returns>
  public static IEnumerable<T> GetFieldsOfType<T>(this Type type)
      where T : class
  {
    return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .Where(f => f.FieldType == typeof(T))
        .Select(f => f.GetValue(null))
        .OfType<T>();
  }
}
