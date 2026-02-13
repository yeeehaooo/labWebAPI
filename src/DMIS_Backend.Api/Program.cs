using DMIS_Backend.Api.Common.Json;
using DMIS_Backend.Api.Common.OpenAPI;
using DMIS_Backend.Api.DependencyInjectionExtensions;
using DMIS_Backend.Api.Pipeline;
using DMIS_Backend.Api.Pipeline.RequestContext;
using DMIS_Backend.Api.Pipeline.Security;
using DMIS_Backend.Api.Pipeline.Workflows;
using Scalar.AspNetCore;

/// <summary>
/// DMIS Backend API 應用程式進入點
/// 負責配置服務、中介軟體管道和應用程式啟動
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddJsonConfig();

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
builder.Services.AddApplicationModules();

// 註冊 Infrastructure 層服務（DB / Encryption / Cache）
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseWorkflowService();

app.UseExceptionHandler();

/// HTTPS 強制
app.UseHttpsRedirection();

app.UseOpenAPI();

/// Authentication（先驗人）
app.UseAuthentication();

/// Authorization（驗證後才能做 policy 判斷）
app.UseAuthorization();

/// Endpoints
app.MapControllers();

app.Run();
