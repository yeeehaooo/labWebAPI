using DMIS_Backend.Application.Core.Abstractions.Commands;
using DMIS_Backend.Application.Core.Decorators;

namespace DMIS_Backend.Api.DependencyInjectionExtensions;

/// <summary>
/// Application 層的依賴注入擴充方法
/// </summary>
public static class ApplicationConfigs
{
  /// <summary>
  /// 註冊 Application 層服務
  /// 自動掃描並註冊所有 UseCase Handler
  /// </summary>
  public static IServiceCollection AddApplicationModules(this IServiceCollection services)
  {
    var assembly = typeof(IUseCaseCommandHandler<,>).Assembly;

    var handlerTypes = assembly
        .GetTypes()
        .Where(t => !t.IsAbstract && !t.IsInterface)
        .SelectMany(t => t.GetInterfaces()
            .Where(i => i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IUseCaseCommandHandler<,>))
            .Select(i => new
            {
              Service = i,
              Implementation = t
            }));

    foreach (var handler in handlerTypes)
    {
      // 1️⃣ 先註冊真正 handler
      services.AddScoped(handler.Implementation);

      // 2️⃣ 再用 factory 註冊 decorator
      services.AddScoped(handler.Service, provider =>
      {
        var inner = (dynamic)provider.GetRequiredService(handler.Implementation);

        var decoratorType = typeof(ExceptionHandlerDecorator<,>)
         .MakeGenericType(handler.Service.GenericTypeArguments);

        return ActivatorUtilities.CreateInstance(
            provider,
            decoratorType,
            inner);
      });
    }

    return services;
  }

}
