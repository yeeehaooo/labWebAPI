namespace DMIS_Backend.Api.Common.OpenAPI;

/// <summary>
/// OpenAPI 配置擴充方法
/// 統一管理 OpenAPI 服務註冊，包含文件資訊、範例轉換器和安全配置
/// </summary>
public static class OpenAPIConfig
{
  /// <summary>
  /// 註冊 OpenAPI 服務
  /// 包含文件資訊設定、範例轉換器和 JWT 安全配置
  /// </summary>
  /// <param name="services">服務集合</param>
  public static void AddOpenAPI(this IServiceCollection services)
  {
    services.AddOpenApi(options =>
        {
          options.AddDocumentTransformer(
              (document, context, cancellationToken) =>
              {
                document.Info = new()
                {
                  Title = "DMIS Backend API",
                  Version = "v1",
                  Description = "DMIS Backend API Documentation",
                };
                return Task.CompletedTask;
              }
          );

          // 註冊 Example Transformer，讓標了 RequestExampleAttribute 和 ResponseExampleAttribute 的 endpoint 自動在 OpenAPI 顯示範例.
          options.AddOperationTransformer(ExampleOperationTransformer.TransformAsync);

          // 註冊 JWT Security Scheme，讓 OpenAPI 顯示 JWT 認證.
          options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
  }

  #region AddOperationTransformer vs AddDocumentTransformer 說明
  // 參考文件: hhttps://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/openapi/customize-openapi?view=aspnetcore-10.0
  /*
      [5 tools called]
      ## `AddOperationTransformer` vs `AddDocumentTransformer`

      ### 1. 作用範圍不同

      | | AddOperationTransformer | AddDocumentTransformer |
      |---|---|---|
      | **對象** | 單一 operation（路徑 + HTTP 方法） | 整份 OpenAPI 文件 |
      | **呼叫次數** | 每個 endpoint 各執行一次 | 每個文件只執行一次 |

      ### 2. 用途不同

      **AddOperationTransformer**
      - 修改「個別 endpoint」的參數、回應等
      - 每個 endpoint 都會觸發一次
      - 適合：為每個操作加上 500 回應、修改某個 endpoint 的 description 等

      **AddDocumentTransformer**
      - 修改「整份文件」的頂層結構
      - 適合：加 `document.Info`、`document.Components.SecuritySchemes`、調整頂層設定等

      ### 3. 執行順序

      轉換器順序是：
      1. Schema transformers（依註冊順序）
      2. Operation transformers（依註冊順序）
      3. Document transformers（最後執行）

      所有 operation 和 schema 都處理完後，才會跑 document transformer，因此 document transformer 可以看到完整文件。

      ### 4. 對應到你專案的用法

      - **ExampleOperationTransformer**：用 `AddOperationTransformer`
      → 要針對「每個 endpoint」補 example，所以用 operation 層級。
      - **BearerSecuritySchemeTransformer**：用 `AddDocumentTransformer`
      → 要在 `document.Components.SecuritySchemes` 加 JWT，是文件頂層結構，用 document 層級。

      簡化來說：改單一 endpoint 用 Operation，改整份文件用 Document。

  */
  #endregion
}
