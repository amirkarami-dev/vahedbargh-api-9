using Coreapi.Api.Middlewares;
using Coreapi.Application;
using Coreapi.Application.Common.Models;
using Coreapi.Common.Models;
using Coreapi.Infrastructure;
using Coreapi.Infrastructure.Identity;
using Coreapi.Infrastructure.Notification;
using Coreapi.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Polly;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

// Logging configuration
builder.Logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
builder.Logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);

builder.Services.Configure<MetaBaseSettings>(builder.Configuration.GetSection("Metabase"));
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole(); // Log to console
//builder.Logging.AddDebug();   // Log to debug output

//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Listen(IPAddress.Any, 80, listenOptions =>
//    {
//        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
//    });
//});


// این تنظیم هنگام دیپلوی برای لیارا می باشد
//builder.WebHost.UseUrls("http://0.0.0.0:5000");


// Add services to the container
var services = builder.Services;

services.AddRazorPages();
services.AddInfrastructure(builder.Configuration);
services.AddPersistence(builder.Configuration);
services.AddApplication();

services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API Documentation",
        Version = "v1",
        Description = "API documentation for the CoreAPI project"
    });
    // Explicitly tell Swagger to treat `UsersInput` correctly
    options.MapType<UsersInput>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = {
            { "User", new OpenApiSchema { Type = "string" } },
            { "RoleId", new OpenApiSchema { Type = "integer" } }
        }
    });
});

services.AddMvc()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
services.AddSignalR().AddJsonProtocol();
services.AddCors();
services.AddApiVersioning(options => options.ReportApiVersions = true);

var app = builder.Build();

// Map Identity APIs
app.MapIdentityApi<ApplicationUser>();

// Middleware to handle query string tokens
app.Use(async (context, next) =>
{
    if (context.Request.QueryString.HasValue)
    {
        if (!context.Request.Headers.Any(c => c.Key == "Authorization"))
        {
            var queryString = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
            var token = queryString.Get("access_token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                context.Request.Headers.Add("Authorization", new[] { $"Bearer {token}" });
            }
        }
    }

    await next.Invoke();
});

// Configure CORS
app.UseCors(policy => policy
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithOrigins("http://localhost:3000")
    .SetIsOriginAllowed(origin => true) // Allow any origin
    .AllowCredentials()
    .WithExposedHeaders("x-access-token", "x-refresh-token"));
app.UseWebSockets();
// Custom exception handler middleware
app.UseCustomExceptionHandler();

// Static files middleware
app.UseStaticFiles();

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = "api"; // Swagger UI available at /api
});

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/frontnotif");
app.MapHub<NotificationHub>("/notif");

// Map controllers
app.MapControllers();

// Database initialization with retry policy
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    try
    {
        var identity = serviceProvider.GetRequiredService<IdentityAppDbContext>();
        var context = serviceProvider.GetRequiredService<CoreapiDbContext>();
        const int retries = 10;
        var retry = Policy.Handle<SqlException>()
            .WaitAndRetry(
                retryCount: retries,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retry, ctx) =>
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogWarning($"Retry {retry} of {retries} failed: {exception.Message}");
                });

        retry.Execute(() =>
        {
            // Uncomment if migrations are required
            // context.Database.Migrate();
            // identity.Database.Migrate();
        });

    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
    }
}

app.Run();
