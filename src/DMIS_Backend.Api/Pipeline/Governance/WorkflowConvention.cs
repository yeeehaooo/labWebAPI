using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DMIS_Backend.Api.Pipeline.Governance;

/// <summary>
/// Workflow 約定
/// 自動為所有 Controller 應用 Workflow 相關的規範
/// </summary>
public class WorkflowConvention : IApplicationModelConvention
{
  /// <summary>
  /// 應用約定到應用程式模型
  /// </summary>
  /// <param name="application">應用程式模型</param>
  public void Apply(ApplicationModel application)
  {
    // 可以在這裡添加全域的 Workflow 規範
    // 例如：自動為所有 Controller 添加特定的 Filter 或 Metadata
  }
}
