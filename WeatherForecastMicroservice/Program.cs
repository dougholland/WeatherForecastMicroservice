namespace WeatherForecastMicroservice
{
    using Azure.Identity;

    using Azure.Messaging.ServiceBus;

    using Azure.Monitor.OpenTelemetry.AspNetCore;

    using Azure.Monitor.OpenTelemetry.Exporter;

    using Microsoft.AspNetCore.Authentication;

    using Microsoft.AspNetCore.Authentication.JwtBearer;

    using Microsoft.AspNetCore.Builder;
    
    using Microsoft.EntityFrameworkCore;
    
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    using Microsoft.Extensions.Logging;

    using Microsoft.Identity.Web;
    
    using Microsoft.OpenApi.Models;

    using OpenTelemetry.Metrics;

    using OpenTelemetry.Resources;

    using OpenTelemetry.Trace;

    using WeatherForecastMicroservice.Entities;

    /// <summary>
    /// Defines the application entry point.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The name of the service.
        /// </summary>
        /// <remarks>Used to define the service name within the OpenTelemetry configuration.</remarks>
        private static readonly string serviceName = "Weather Forecast Microservice";

        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="args">An array of arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // configure cross-origin resource sharing (CORS).
            ConfigureCORS(builder);

            // configure in-memory cache - alternatively, configure distributed cache if service is expected to scale to multiple instances.
            builder.Services.AddMemoryCache();

            // add controllers to the service.
            builder.Services.AddControllers();

            // learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            ConfigureAzureKeyVault(builder);

            ConfigureAzureServiceBus(builder);

            ConfigureCORS(builder);

            ConfigureOpenTelemetry(builder);

            ConfigureHealthChecks(builder);

            ConfigureAuthentication(builder);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Weather Forecast Microservice",
                    Version = "v1"
                });
            });

            builder.Services.AddDbContextFactory<WeatherForecastDbContext>(options =>
            {
                // Azure SQL connection string is stored within Azure Key Vault and read from Azure App Configuration.
                var connectionString = builder.Configuration["AzureSqlConnection"];
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("The connection string could not be read from Azure App Configuration.");
                }

                options.UseSqlServer(connectionString);
            });

            builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

            var app = builder.Build();

            app.UseSwagger();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();

                // use the developer exception page in development.
                app.UseDeveloperExceptionPage();

                app.UseCors("AnyOrigin");
            }
            else
            {
                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swagger, request) => {
                        swagger.Servers.Clear();
                        swagger.Servers.Add(new OpenApiServer { Url = $"{request.Scheme}://{request.Host.Value}" });
                    });
                });

                // use the exception handler in other environments.
                app.UseExceptionHandler("/error");

                app.UseCors();
            }

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseHealthChecks("/api/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
            {
                Predicate = registration => registration.Tags.Contains("liveness")
            });

            app.MapControllers();

            app.Run();
        }

        /// <summary>
        /// Configures the Azure Key Vault to be used for secrets management.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Secrets within Azure Key Vault are accessed within the microservice via Azure App Configuration.
        /// </para>
        /// <para>
        /// Within Azure App Configuration, choose Create | Key Vault reference to reference secrets defined within Azure Key Vault.
        /// </para>
        /// </remarks>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.WebApplicationBuilder"/> instance used to build the web application or services.</param>
        private static void ConfigureAzureKeyVault(WebApplicationBuilder builder)
        {
            builder.Configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect(builder.Configuration["AzureAppConfiguration:ConnectionString"])
                    .ConfigureKeyVault(vault =>
                    {
                        vault.SetCredential(new DefaultAzureCredential());
                    });
            });
        }

        /// <summary>
        /// Configures authentication to use the <see cref="T:MockAuthenticationHandler"/> within development or the production authentication handler.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.WebApplicationBuilder"/> instance used to build the web application or services.</param>
        private static void ConfigureAuthentication(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddAuthentication("Development")
                    .AddScheme<AuthenticationSchemeOptions, MockAuthenticationHandler>("Development", options => { });
            }
            else
            {
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("EntraID"));

                builder.Services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var handler = options.Events.OnTokenValidated;

                    options.Events.OnTokenValidated = async context =>
                    {
                        await handler(context);
                    };
                });
            }
        }

        /// <summary>
        /// Configures the service for Cross Origin Resource Sharing (CORS).
        /// </summary>
        /// <remarks>Cross Origin Resource Sharing should be configured within Azure API management if that service is used.</remarks>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.WebApplicationBuilder"/> instance used to build the web application or services.</param>
        private static void ConfigureCORS(WebApplicationBuilder builder)
        {
            // configure CORS where required although this should be configured in Azure API management instead.
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(builder.Configuration["AllowedOrigins"] ?? string.Empty);
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });

                options.AddPolicy(name: "AnyOrigin", policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });
        }

        /// <summary>
        /// Configures the use of ASP.NET health checks.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.WebApplicationBuilder"/> instance used to build the web application or services.</param>
        private static void ConfigureHealthChecks(WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                            .AddCheck("LivenessHealthCheck", () => HealthCheckResult.Healthy(), tags: new[] { "liveness" });
        }

        /// <summary>
        /// Configures the use of Open Telemetry.
        /// </summary>
        /// <remarks>The Azure Monitor connection string must be configured within appsettings.json.</remarks>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.WebApplicationBuilder"/> instance used to build the web application or services.</param>
        private static void ConfigureOpenTelemetry(WebApplicationBuilder builder)
        {
            // configure open telemetry metrics.
            builder.Services.AddOpenTelemetry().WithMetrics(metrics => metrics
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddAzureMonitorMetricExporter()

                // limits the number of metric points per metric stream.
                .SetMaxMetricPointsPerMetricStream(500)

                // limits the number of metric streams that can be active at any one time.
                .SetMaxMetricStreams(5)

                // configure the console exporter, optionally use ConsoleExporterOutputTargets to configure the output of the exporter.
                .AddConsoleExporter()
            );

            // configure open telemetry tracing.
            builder.Services.AddOpenTelemetry().WithTracing(tracing => tracing
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                .AddAspNetCoreInstrumentation(options => options
                    .EnrichWithHttpRequest = (activity, request) =>
                    {
                        if (request.HttpContext.User.Identity?.IsAuthenticated == true)
                        {
                            activity.SetTag("username", request.HttpContext.User.Identity.Name);
                        }

                        activity.SetTag("method", request.Method);

                        activity.SetTag("protocol", request.Protocol);

                        activity.SetTag("path", request.Path);

                        if (request.QueryString.HasValue)
                        {
                            activity.SetTag("query", request.QueryString.Value);
                        }
                    }
                )
                .AddHttpClientInstrumentation()
                .AddAzureMonitorTraceExporter()
                .AddConsoleExporter()

                // configure the sampling rate to 10%.
                .SetSampler(new TraceIdRatioBasedSampler(0.1))

                // configure the console exporter, optionally use ConsoleExporterOutputTargets to configure the output of the exporter.
                .AddConsoleExporter()
            );

            // configure open telemetry to use azure monitor.
            builder.Services.AddOpenTelemetry().UseAzureMonitor(options => options
                .ConnectionString = builder.Configuration["AzureMonitor:ConnectionString"]
            );

            builder.Logging.ClearProviders();

            // configure logging to use open telemetry.
            builder.Logging.AddOpenTelemetry();
        }

        /// <summary>
        /// Configures the use of Azure Service Bus.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.AspNetCore.Builder.WebApplicationBuilder"/> instance used to build the web application or services.</param>
        private static void ConfigureAzureServiceBus(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ServiceBusClient>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();

                var connectionString = configuration?["AzureServiceBus:ConnectionString"];

                var queue = configuration?["AzureServiceBus:Queue"];

                return new ServiceBusClient(connectionString);
            });
        }
    }
}
