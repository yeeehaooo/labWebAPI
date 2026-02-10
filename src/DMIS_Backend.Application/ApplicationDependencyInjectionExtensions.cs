using DMIS_Backend.Application.Kernel.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DMIS_Backend.Application;

/// <summary>
/// Application 層的依賴注入擴充方法
/// </summary>
public static class ApplicationDependencyInjectionExtensions
{
  /// <summary>
  /// 註冊 Application 層服務
  /// 自動掃描並註冊所有 UseCase Handler
  /// </summary>
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    var assembly = typeof(ApplicationDependencyInjectionExtensions).Assembly;
    var commandHandlerInterface = typeof(IUseCaseCommandHandler<,>);
    var queryHandlerInterface = typeof(IUseCaseQueryHandler<,>);

    // 註冊 Command Handlers
    var commandHandlers = assembly
        .GetTypes()
        .Where(t =>
            !t.IsAbstract &&
            !t.IsInterface &&
            t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == commandHandlerInterface))
        .ToList();

    foreach (var handler in commandHandlers)
    {
      var interfaces = handler.GetInterfaces()
          .Where(i =>
              i.IsGenericType &&
              i.GetGenericTypeDefinition() == commandHandlerInterface);

      foreach (var @interface in interfaces)
      {
        services.AddScoped(@interface, handler);
      }
    }

    // 註冊 Query Handlers
    var queryHandlers = assembly
        .GetTypes()
        .Where(t =>
            !t.IsAbstract &&
            !t.IsInterface &&
            t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == queryHandlerInterface))
        .ToList();

    foreach (var handler in queryHandlers)
    {
      var interfaces = handler.GetInterfaces()
          .Where(i =>
              i.IsGenericType &&
              i.GetGenericTypeDefinition() == queryHandlerInterface);

      foreach (var @interface in interfaces)
      {
        services.AddScoped(@interface, handler);
      }
    }

    // 註冊 Validators（自動掃描所有以 Validator 結尾的類別）
    var validators = assembly
        .GetTypes()
        .Where(t =>
            !t.IsAbstract &&
            !t.IsInterface &&
            t.Name.EndsWith("Validator", StringComparison.Ordinal))
        .ToList();

    foreach (var validator in validators)
    {
      services.AddScoped(validator);
    }

    return services;
  }
}
