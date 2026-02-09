namespace DMIS_Backend.Application.Kernel.Guards;

public sealed class DiagnosticInfo
{
  public string? DiagnosticMessage { get; }

  public DiagnosticInfo(string? diagnosticInfo)
  {
    DiagnosticMessage = diagnosticInfo;
  }
}
