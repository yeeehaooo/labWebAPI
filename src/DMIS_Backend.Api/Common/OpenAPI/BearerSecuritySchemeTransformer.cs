using System.Collections.Frozen;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace DMIS_Backend.Api.Common.OpenAPI;

/// <summary>
/// Bearer Security Scheme 文件轉換器
/// 自動為 OpenAPI 文件添加 JWT Bearer 認證配置，並為需要認證的端點標記安全需求
/// </summary>
/// <remarks>
/// 參考文件: https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/openapi/customize-openapi?view=aspnetcore-9.0
/// </remarks>
public class BearerSecuritySchemeTransformer(
  IAuthenticationSchemeProvider authenticationSchemeProvider
) : IOpenApiDocumentTransformer
{
  /// <summary>
  /// 轉換 OpenAPI 文件，添加 JWT Bearer 安全配置
  /// </summary>
  /// <param name="document">OpenAPI 文件</param>
  /// <param name="context">轉換上下文</param>
  /// <param name="cancellationToken">取消令牌</param>
  public async Task TransformAsync(
    OpenApiDocument document,
    OpenApiDocumentTransformerContext context,
    CancellationToken cancellationToken
  )
  {
    var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
    if (!authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
      return;

    var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme>
    {
      ["Bearer"] = new OpenApiSecurityScheme
      {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        In = ParameterLocation.Header,
        BearerFormat = "JWT",
      },
    };
    document.Components ??= new OpenApiComponents();
    document.Components.SecuritySchemes = securitySchemes;

    // 為需要認證的 endpoints 加上 Bearer 為必輸
    var authorizedEndpoints = GetAuthorizedEndpoints(context.DescriptionGroups);
    foreach (var (path, pathItem) in document.Paths ?? [])
    {
      foreach (var (operationType, operation) in pathItem.Operations ?? [])
      {
        if (!authorizedEndpoints.Contains((NormalizePath(path), operationType)))
          continue;

        operation.Security ??= [];
        operation.Security.Add(
          new OpenApiSecurityRequirement
          {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
          }
        );
      }
    }
  }

  private static FrozenSet<(string Path, HttpMethod Method)> GetAuthorizedEndpoints(
    IReadOnlyList<ApiDescriptionGroup>? descriptionGroups
  )
  {
    if (descriptionGroups is null or [])
      return FrozenSet<(string, HttpMethod)>.Empty;

    var authorized = new HashSet<(string Path, HttpMethod Method)>();

    foreach (var group in descriptionGroups)
    {
      foreach (var api in group.Items)
      {
        if (!RequiresAuthorization(api))
          continue;

        if (ParseHttpMethod(api.HttpMethod) is { } method)
          authorized.Add((NormalizePath(api.RelativePath), method));
      }
    }

    return authorized.ToFrozenSet();
  }

  private static bool RequiresAuthorization(ApiDescription api)
  {
    var metadata = api.ActionDescriptor?.EndpointMetadata;
    if (metadata is null)
      return false;

    var hasAllowAnonymous = metadata.OfType<IAllowAnonymous>().Any();
    if (hasAllowAnonymous)
      return false;

    return metadata.OfType<IAuthorizeData>().Any();
  }

  private static string NormalizePath(string? path)
  {
    if (string.IsNullOrEmpty(path))
      return "/";
    return path.StartsWith('/') ? path : "/" + path;
  }

  private static HttpMethod? ParseHttpMethod(string? httpMethod) =>
    httpMethod?.ToUpperInvariant() switch
    {
      "GET" => HttpMethod.Get,
      "POST" => HttpMethod.Post,
      "PUT" => HttpMethod.Put,
      "DELETE" => HttpMethod.Delete,
      "PATCH" => HttpMethod.Patch,
      "HEAD" => HttpMethod.Head,
      "OPTIONS" => HttpMethod.Options,
      _ => null,
    };
}
