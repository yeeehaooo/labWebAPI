using System.Reflection;

namespace DMIS_Backend.Domain.Kernel.Primitives;

public abstract record Code
{
  public string Value { get; }
  public string Name { get; }
  protected Code(string value, string name)
  {
    Value = value;
    Name = name;
  }
  public override string ToString() => Value;

  public static implicit operator string(Code code)
      => code.Value;


  // ===== Smart Enum 功能 =====
  private static readonly Dictionary<string, Code> _byValue;
  private static readonly Dictionary<string, Code> _byName;

  static Code()
  {
    var fields = typeof(Code)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(f => f.FieldType == typeof(Code))
        .Select(f => (Code)f.GetValue(null)!);

    _byValue = fields.ToDictionary(x => x.Value);
    _byName = fields.ToDictionary(x => x.Name);
  }
  public static IReadOnlyCollection<Code> All =>
      _byValue.Values;

  public static Code FromValue(string value)
       => _byValue.TryGetValue(value, out var code)
           ? code
           : throw new ArgumentException($"Invalid workflow value: {value}");

  public static Code FromName(string name)
      => _byName.TryGetValue(name, out var code)
          ? code
          : throw new ArgumentException($"Invalid workflow name: {name}");

  public static bool TryFromValue(string value, out Code? code)
      => _byValue.TryGetValue(value, out code);
}
public abstract record ErrorCode : Code
{
  public string Message { get; }
  public ErrorSource Source { get; }

  public ErrorCode(
      string value,
      string name,
      string message,
      ErrorSource source)
      : base(value, name)
  {
    Message = message;
    Source = source;
  }
}

public sealed record SystemCode : ErrorCode
{

  private SystemCode(string value, string name, string message)
      : base(value, name, message, ErrorSource.System)
  {
  }
  public static readonly SystemCode Success =
        new("00000", "Success", "Success");

  public static readonly SystemCode Authentication =
      new("40001", "Authentication", "Authentication error");

  public static readonly SystemCode InternalError =
      new("50000", "InternalError", "Internal server error");

  public static readonly SystemCode DatabaseError =
      new("50001", "DatabaseError", "Database error");

  public static readonly SystemCode Timeout =
      new("50002", "Timeout", "Operation timeout");

  public static readonly SystemCode UnknownError =
      new("59999", "UnknownError", "Unknown system error");
}

public sealed record ApplicationCode : ErrorCode
{

  private ApplicationCode(string value, string name, string message)
      : base(value, name, message, ErrorSource.Application)
  {
  }

  public static readonly ApplicationCode NotFound =
        new("A0001", "NotFound", "Resource not found");

  public static readonly ApplicationCode AlreadyExists =
      new("A0002", "AlreadyExists", "Resource already exists");

  public static readonly ApplicationCode InvalidState =
      new("A0003", "InvalidState", "Invalid state");

  public static readonly ApplicationCode OperationFailed =
      new("A0004", "OperationFailed", "Operation failed");

  public static readonly ApplicationCode ValidationFailed =
      new("A0005", "ValidationFailed", "Validation failed");
}

public sealed record DomainCode : ErrorCode
{
  private DomainCode(string value, string name, string message)
      : base(value, name, message, ErrorSource.Domain)
  {
  }
  public static readonly DomainCode Required =
        new("D0001", "Required", "Required field");

  public static readonly DomainCode InvalidFormat =
      new("D0002", "InvalidFormat", "Invalid format");

  public static readonly DomainCode OutOfRange =
      new("D0003", "OutOfRange", "Value out of range");

  public static readonly DomainCode RuleViolation =
      new("D0004", "RuleViolation", "Domain rule violated");
}
