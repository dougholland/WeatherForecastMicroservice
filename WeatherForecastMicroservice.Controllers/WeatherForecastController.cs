namespace WeatherForecastMicroservice.Controllers
{
    using Azure.Messaging.ServiceBus;

    using Microsoft.AspNetCore.Authorization;

    using Microsoft.AspNetCore.Mvc;
    
    using Microsoft.Extensions.Configuration;
    
    using Microsoft.Extensions.Logging;

    using Microsoft.Identity.Web.Resource;
    
    using OpenTelemetry.Trace;

    using WeatherForecastMicroservice.Entities;
    
    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// Weather forecast controller.
    /// </summary>
    [ApiController]
    [Route("/api")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        /// <summary>
        /// The trace provider to be used by the controller.
        /// </summary>
        private readonly TracerProvider tracerProvider;

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
        /// The weather forecast repository to be used by the controller.
        /// </summary>
        private readonly IWeatherForecastRepository repository;

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
        /// <param name="configuration">The configuration to be used by the controller.</param>
        /// <param name="serviceBus">The service bus client to be used by the controller.</param>
        
        public WeatherForecastController(ILogger<WeatherForecastController> logger, TracerProvider tracerProvider, ServiceBusClient serviceBus, IConfiguration configuration, IWeatherForecastRepository repository)
        {
            this.logger = logger;

            this.tracerProvider = tracerProvider;

            this.serviceBus = serviceBus;

            this.configuration = configuration;

            this.repository = repository;
        }

        /// <summary>
        /// Gets a list of weather forecasts.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation to get all forecasts. The task result contains all the weather forecasts.</returns>
        [RequiredScope("WeatherForecastMicroservice")]
        [HttpGet("WeatherForecasts", Name = "GetWeatherForecasts")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 120)]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecastsAsync()
        {
            var tracer = this.tracerProvider.GetTracer(this.GetType().FullName);

            using var span = tracer.StartActiveSpan(nameof(GetWeatherForecastsAsync));

            var sender = this.serviceBus.CreateSender(this.configuration["AzureServiceBus:Queue"]);

            var message = new ServiceBusMessage("GetWeatherForecastsAsync");

            await (sender?.SendMessageAsync(message) ?? Task.CompletedTask);

            return Ok(await this.repository.GetForecastsAsync());
        }

        /// <summary>
        /// Gets a weather forecasts based on the given identifier.
        /// </summary>
        /// <param name="id">The identifier of the weather forecast.</param>
        /// <returns>A task that represents the asynchronous operation to get a weather forecast. The task result contains the weather forecast.</returns>
        /// <exception cref="NotImplementedException"></exception>
        [RequiredScope("WeatherForecastMicroservice")]
        [HttpGet("WeatherForecast{id}", Name = "GetWeatherForecast")]
        public async Task<ActionResult<WeatherForecast>> GetWeatherForecast(int id)
        {
            var forecast = await this.repository.GetForecastByIdAsync(id);
            
            if (forecast == null)
            {
                return NotFound();
            }

            return Ok(forecast);
        }

        /// <summary>
        /// Posts a weather forecast.
        /// </summary>
        /// <param name="forecast">The forecast to be created within the repository.</param>
        /// <returns></returns>
        [RequiredScope("WeatherForecastMicroservice")]
        [HttpPost("WeatherForecast", Name = "PostWeatherForecast")]
        public async Task<IActionResult> PostWeatherForecastAsync([FromBody] WeatherForecast forecast)
        {
            if (forecast == null)
            {
                return BadRequest("Weather forecast is null.");
            }

            await this.repository.SaveForecastAsync(forecast);

            return CreatedAtAction("GetWeatherForecast", new { id = forecast.Id }, forecast);
        }

        /// <summary>
        /// Deletes a weather forecast based on the given identifier.
        /// </summary>
        /// <param name="id">The identifier of the weather forecast.</param>
        /// <returns>A task that represents the asynchronous operation to delete a weather forecast. The task result indicates whether the weather forecast was successfully deleted.</returns>
        [HttpDelete("WeatherForecast{id}", Name = "DeleteWeatherForecast")]
        public async Task<IActionResult> DeleteWeatherForecastAsync(int id)
        {
            var forecast = await this.repository.GetForecastByIdAsync(id);

            if (forecast == null)
            {
                return NotFound();
            }

            await this.repository.DeleteForecastAsync(forecast);

            return NoContent();
        }

        /// <summary>
        /// Gets an exception to test the ASP.NET Web API error handling endpoint.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation to get an error.</returns>
        /// <exception cref="Exception">An exception to test the ASP.NET Web API error handling endpoint.</exception>
        [RequiredScope("WeatherForecastMicroservice")]
        [HttpGet("Error", Name = "GetError")]
        [ResponseCache(Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetErrorAsync()
        {
            // simulate an asynchronous operation.
            await Task.Yield();

            throw new Exception("Testing error routing.");
        }
    }
}

