using DMIS_Backend.Domain.Kernel.Primitives;

namespace DMIS_Backend.Application.Core.Workflows;

//public static class WorkflowConstants
//{
//  public const string System = "SYSM";
//  public const string Auth = "AUTH";
//  public const string Create = "WF01";
//  public const string Update = "WF02";
//  public const string Delete = "WF03";
//  public const string Query = "WF04";
//  public const string Login = "WF05";
//  public const string Checkout = "WF06";

//  public static WorkflowCode FromValue(string value)
//      => value switch
//      {
//        WorkflowConstants.System => WorkflowCode.System,
//        WorkflowConstants.Auth => WorkflowCode.Auth,
//        WorkflowConstants.Create => WorkflowCode.Create,
//        WorkflowConstants.Update => WorkflowCode.Update,
//        WorkflowConstants.Delete => WorkflowCode.Delete,
//        WorkflowConstants.Query => WorkflowCode.Query,
//        WorkflowConstants.Login => WorkflowCode.Login,
//        WorkflowConstants.Checkout => WorkflowCode.Checkout,
//        _ => throw new ArgumentException($"Invalid workflow value: {value}")
//      };
//}
public sealed record WorkflowCode : Code
{
  public WorkflowCode(string value, string name)
      : base(value, name)
  {
  }
  public string Build(Code errorCode)
      => $"{Value}-{errorCode.Value}";

  public static readonly WorkflowCode System =
     new("SYSM", "System");

  public static readonly WorkflowCode Auth =
     new("AUTH", "Auth");

  public static readonly WorkflowCode Create =
      new("WF01", "Create");

  public static readonly WorkflowCode Update =
      new("WF02", "Update");

  public static readonly WorkflowCode Delete =
      new("WF03", "Delete");

  public static readonly WorkflowCode Query =
      new("WF04", "Query");

  public static readonly WorkflowCode Login =
      new("WF05", "Login");

  public static readonly WorkflowCode Checkout =
      new("WF06", "Checkout");

}
