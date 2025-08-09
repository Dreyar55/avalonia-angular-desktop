using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System.Net;
using System.Net.Sockets;

namespace DesktopApp;

#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code

public class WebAppMetadata
{
    public required WebApplication WebApplication { get; set; }
    public required Uri BackendUri { get; set; }
    public required Uri FrontendUri { get; set; }
}

public static class WebApplicationCreator
{
    public static WebAppMetadata CreateWebApp()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.File("/logs/desktopapp_logs-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Application starting up...");

            var address = GetAvailableHttpEndpoint();
            var builder = WebApplication.CreateBuilder();
            var frontendPath = Path.Combine(builder.Environment.WebRootPath!, "dist", "browser");

            builder.Logging.ClearProviders();
            builder.WebHost.UseUrls(address);
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext();
            });

            builder.Services.AddCors();

            var app = builder.Build();

            app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseRouting();

            // These !DEBUG directives just serve the frontend application (in this case, Angular) statically via the ASP.NET Core web server
            // The directory as well as index.html will be present ONLY if you run `npm run build` first.
            // Build the dotnet app in `Release` mode to enable it, otherwise it will always default to ng serve's http://localhost:4200/
#if !DEBUG
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = new PhysicalFileProvider(frontendPath),
                DefaultFileNames = ["index.html"],
                RequestPath = "",
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(frontendPath),
                RequestPath = "",
            });
#endif

            app.MapGet("api/metadata", ([FromServices] IHostEnvironment environment) =>
            {
                var metadata = new ApiMetadataResponse
                {
                    Name = "DesktopApp",
                    Version = "1.0.0",
                    Environment = environment.EnvironmentName,
                };
                return metadata;
            });

#if !DEBUG
            app.MapFallbackToFile("index.html", new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(frontendPath),
            });
#endif

            // Start API on a separate thread
            _ = Task.Run(async () => await app.RunAsync());

            return new()
            {
                WebApplication = app,
                BackendUri = new(address),
#if !DEBUG
                FrontendUri = new(address),
#else
                FrontendUri = new("http://localhost:4200/"),
#endif
            };
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, ex.Message);
            throw;
        }
        finally
        {
            Log.Information("Application shutting down...");
            Log.CloseAndFlush();
        }
    }

    public static int GetAvailableHttpPort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    public static bool IsPortAvailable(int port)
    {
        try
        {
            using var tcpListener = new TcpListener(IPAddress.Loopback, port);
            tcpListener.Start();
            return true;
        }
        catch (SocketException)
        {
            return false;
        }
    }

    private static string GetAvailableHttpEndpoint()
    {
        const int defaultPort = 54321;
        int serverPort = defaultPort;
        if (!IsPortAvailable(serverPort))
        {
            serverPort = GetAvailableHttpPort();
            Log.Warning("Default port {DefaultAppPort} is unavailable, picking a new port ({NewServerPort})...", defaultPort, serverPort);
        }

        return $"http://localhost:{serverPort}/";
    }

    public class ApiMetadataResponse
    {
        public required string Name { get; set; }
        public required string Version { get; set; }
        public required string Environment { get; set; }
    }

}
