namespace WeatherForecastMicroservice.Controllers
{
    using Azure.Messaging.ServiceBus;

    using Microsoft.AspNetCore.Authorization;

    using Microsoft.AspNetCore.Mvc;
    
    using Microsoft.Extensions.Caching.Memory;
    
    using Microsoft.Extensions.Configuration;
    
    using Microsoft.Extensions.Logging;

    using Microsoft.Identity.Web.Resource;
    
    using OpenTelemetry.Trace;
        
    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// Weather forecast controller.
    /// </summary>
    [ApiController]
    [Route("/")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        /// <summary>
        /// The trace provider to be used by the controller.
        /// </summary>
        private readonly TracerProvider tracerProvider;

        /// <summary>
        /// The memory cache to be used by the controller.
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// The cache key for weather forecasts.
        /// </summary>
        private readonly string cacheKey = typeof(WeatherForecastController).Name;

        /// <summary>
        /// The service bus client to be used by the controller.
        /// </summary>
        private readonly ServiceBusClient serviceBus;

        /// <summary>
        /// The logger to be used for logging within the controller.
        /// </summary>
        private readonly ILogger<WeatherForecastController> logger;

        /// <summary>
        /// The configuration to be used by the controller.
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Sample list of weather conditions.
        /// </summary>
        private readonly List<string> Conditions =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        /// <summary>
        /// Creates an instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="logger">The logger to be used by the controller.</param>
        /// <param name="tracerProvider">The tracer provider to be used by the controller.</param>
        public WeatherForecastController(IMemoryCache memoryCache, ILogger<WeatherForecastController> logger, TracerProvider tracerProvider, ServiceBusClient serviceBus, IConfiguration configuration)
        {
            this.memoryCache = memoryCache;

            this.logger = logger;

            this.tracerProvider = tracerProvider;

            this.serviceBus = serviceBus;

            this.configuration = configuration;
        }

        /// <summary>
        /// Gets a list of weather forecasts.
        /// </summary>
        /// <returns>The list of weather forecasts.</returns>
        [RequiredScope("WeatherForecastMicroservice")]
        [HttpGet("Forecasts", Name = "GetWeatherForecast")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 120)]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecastsAsync()
        {
            var tracer = this.tracerProvider.GetTracer(this.GetType().FullName);

            using var span = tracer.StartActiveSpan(nameof(GetWeatherForecastsAsync));

            var sender = this.serviceBus.CreateSender(this.configuration["AzureServiceBus:Queue"]);

            var message = new ServiceBusMessage("GetWeatherForecastsAsync");

            await (sender?.SendMessageAsync(message) ?? Task.CompletedTask);

            return await Task.Run(() =>
            {
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Conditions[Random.Shared.Next(Conditions.Count)]
                })
                .ToArray();
            });
        }

        /// <summary>
        /// Gets a list of cached weather forecasts.
        /// </summary>
        /// <returns>The list of cached weather forecasts.</returns>
        [RequiredScope("WeatherForecastMicroservice")]
        [HttpGet("CachedForecasts", Name = "GetCachedWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetCachedWeatherForecastsAsync()
        {
            // establish an instance of the OpenTelemetry tracer.
            var tracer = this.tracerProvider.GetTracer(this.GetType().FullName);

            using var span = tracer.StartActiveSpan(nameof(GetWeatherForecastsAsync));

            if (!memoryCache.TryGetValue<IEnumerable<WeatherForecast>>(cacheKey, out var weatherForecasts))
            {
                weatherForecasts = await Task.Run(() =>
                {
                    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Conditions[Random.Shared.Next(Conditions.Count)]
                    })
                    .ToArray();
                });
            }

            if (weatherForecasts == null)
            {
                return NotFound();
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));

            memoryCache.Set<IEnumerable<WeatherForecast>>(cacheKey, weatherForecasts, cacheEntryOptions);

            return Ok(weatherForecasts);
        }

        /// <summary>
        /// Gets an exception to test the ASP.NET Web API error handling endpoint.
        /// </summary>
        /// <returns>An exception is thrown and therefore there is no return value.</returns>
        /// <exception cref="Exception">An exception to test the ASP.NET Web API error handling endpoint.</exception>
        [RequiredScope("WeatherForecastMicroservice")]
        [Route("/error/test")]
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetErrorAsync()
        {
            // simulate an asynchronous operation.
            await Task.Yield();

            throw new Exception("Testing error routing.");
        }
    }
}

