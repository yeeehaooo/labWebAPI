namespace DMIS_Backend.Api.Common.Workflows;

public sealed record WorkflowContext(
  string Module,
  string Operation,
  string Function)
{
  public string Build(string errorCode)
    => $"{Module}-{Operation}-{Function}-{errorCode}";
}
