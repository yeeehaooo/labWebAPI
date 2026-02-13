using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DMIS_Backend.Api.Common.Json;

public static class JsonConfig
{
  public static void AddJsonConfig(this WebApplicationBuilder builder)
  {
    // 配置 JSON 選項（駝峰命名、中文不編碼）
    builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
      });
  }
}
