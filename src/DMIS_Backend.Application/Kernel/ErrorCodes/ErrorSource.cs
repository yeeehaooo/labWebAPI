//using System.Runtime.CompilerServices;

//namespace DMIS_Backend.Application.Kernel.ErrorCodes;

///// <summary>
///// 產生錯誤來源描述（Handler 層級）
/////
///// 設計目標：
///// - 精準指到「哪一個 Handler 的哪一行」
///// - 不依賴 StackTrace（避免 Pipeline / Decorator 污染）
///// - 可安全在 Release / Production 使用
///// </summary>
//public static class ErrorSource
//{
//  /// <summary>
//  /// 取得目前呼叫位置（通常是 HandlerSourceDecorator 呼叫）
//  ///
//  /// 產出格式：
//  ///   CreateProductCommanHandler.cs:HandleAsync:42
//  /// </summary>
//  public static string FromCaller(
//      [CallerFilePath] string filePath = "",
//      [CallerMemberName] string memberName = "",
//      [CallerLineNumber] int lineNumber = 0)
//  {
//    var fileName = Path.GetFileName(filePath);

//    return $"{fileName}:{memberName}:{lineNumber}";
//  }
//}
