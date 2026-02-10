using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using DMIS_Backend.Api.Common.OpenAPI;
using DMIS_Backend.Api.Common.RequestContext;
using DMIS_Backend.Api.Pipeline;
using DMIS_Backend.Api.Pipeline.Security;
using DMIS_Backend.Application;
using DMIS_Backend.Infrastructure;
using Scalar.AspNetCore;

/// <summary>
/// DMIS Backend API 應用程式進入點
/// 負責配置服務、中介軟體管道和應用程式啟動
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 配置 JSON 選項（駝峰命名、中文不編碼）
builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
  });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Request Context Core（Workflow、User、Tenant 等請求上下文資訊）
builder.Services.AddRequestContextCore();

// Pipeline 相關服務註冊（Exception、Result、Logging 等流程處理）
builder.Services.AddApiPipeline();

// JWT 認證配置
builder.Services.AddJwtAuth(builder.Configuration);

builder.Services.AddOpenAPI();

// 註冊 Application 層服務（UseCase Handlers）
builder.Services.AddApplication();

// 註冊 Infrastructure 層服務（DB / Encryption / Cache）
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

/// 使用 Request Context Core（必須在 UseExceptionHandler 之前）
/// 確保即使請求在到達 Action Filter 之前就失敗，也會有預設的 WorkflowContext
app.UseRequestContextCore();

/// 使用 API Pipeline（包含 Exception Handler 和 Middlewares）
/// Pipeline 會自動處理 RequestContext（透過 Action Filter）
app.UseApiPipeline();

/// HTTPS 強制
app.UseHttpsRedirection();

/// Swagger（開發環境）
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  // 添加 Scalar UI 配置
  app.MapScalarApiReference(options =>
  {
    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
  });
}

/// Authentication（先驗人）
app.UseAuthentication();

/// Authorization（驗證後才能做 policy 判斷）
app.UseAuthorization();

/// Endpoints
app.MapControllers();

app.Run();
