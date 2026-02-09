//namespace DMIS_Backend.Domain.Kernel.Attributes;

///// <summary>
///// FunctionCode Attribute
///// 用於標記 Handler 的功能碼，用於組合完整錯誤碼
///// </summary>
///// <remarks>
///// 範例：
///// <code>
///// [FunctionCode("020")]
///// public class CreateProductCommandHandler : IUseCaseCommandHandler&lt;CreateProductCommand, CreateProductResult&gt;
///// {
/////     // ...
///// }
///// </code>
///// </remarks>
//[AttributeUsage(AttributeTargets.Class)]
//public sealed class FunctionCodeAttribute : Attribute
//{
//  /// <summary>
//  /// 功能碼（例如 "020"）
//  /// </summary>
//  public string Code { get; }

//  /// <summary>
//  /// 初始化 FunctionCodeAttribute
//  /// </summary>
//  /// <param name="code">功能碼</param>
//  public FunctionCodeAttribute(string code)
//  {
//    Code = code ?? throw new ArgumentNullException(nameof(code));
//  }
//}
