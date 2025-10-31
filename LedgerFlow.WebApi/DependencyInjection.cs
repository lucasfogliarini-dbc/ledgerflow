using LedgerFlow.WebApi;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebApi(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplication();
        //builder.Services.AddControllers();
        builder.Services.AddEndpoints();
        builder.AddHealthChecks();
        builder.Services.AddProblemDetails();
        builder.AddOutputCache();
        builder.Services.AddOpenApi();
    }
    public static void UseWebApi(this WebApplication app)
    {
        app.UseOutputCache();//precisa ser antes do MapEndpoints
        //app.MapControllers();
        app.MapEndpoints();
        //app.MapHealthChecks();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        //app.UseAuthorization();
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var endpointTypes = typeof(Program).Assembly
            .DefinedTypes
            .Where(type => !type.IsAbstract
                           && !type.IsInterface
                           && typeof(IEndpoint).IsAssignableFrom(type))
            .Select(type => ServiceDescriptor.Scoped(typeof(IEndpoint), type));

        services.TryAddEnumerable(endpointTypes);

        return services;
    }
    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        IEnumerable<IEndpoint> endpoints = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }
    private static void AddHealthChecks(this WebApplicationBuilder builder)
    {
        //builder.Services.AddLedgerFlowDbContextCheck();
    }
    private static void MapHealthChecks(this WebApplication app)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true, 
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    version,
                    app.Environment.EnvironmentName,
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        description = entry.Value.Description,
                    })
                });

                await context.Response.WriteAsync(result);
            }
        });
    }
    private static void AddOutputCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(policy =>
            {
                policy.Expire(TimeSpan.FromSeconds(30));
            });
            options.AddPolicy("per-user", policy =>
            {
                policy.VaryByValue(context =>
                {
                    var userId = context.User.FindFirst("sub")?.Value ?? "anonymous";
                    return new KeyValuePair<string, string>("userId", userId);
                });
            });
        });
    }
}