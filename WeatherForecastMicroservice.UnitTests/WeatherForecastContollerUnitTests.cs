namespace WeatherForecastMicroservice.UnitTests
{
    using Azure.Messaging.ServiceBus;
    
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.EntityFrameworkCore;
    
    using Microsoft.Extensions.Caching.Memory;

    using Microsoft.Extensions.Configuration;

    using Microsoft.Extensions.Logging;

    using Moq;

    using OpenTelemetry.Trace;

    using WeatherForecastMicroservice.Controllers;

    using WeatherForecastMicroservice.Entities;

    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// Provides unit tests for the <see cref="WeatherForecastController"/> class.
    /// </summary>
    [TestClass]
    public class WeatherForecastContollerUnitTests
    {
        /// <summary>
        /// The options for the database context.
        /// </summary>
        private DbContextOptions<WeatherForecastDbContext>? options;

        /// <summary>
        /// The weather forecast controller to be used for the unit tests.
        /// </summary>
        private WeatherForecastController? controller;

        /// <summary>
        /// Provides initialization for unit tests.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var memoryCache = new Mock<IMemoryCache>().Object;

            var logger = new Mock<ILogger<WeatherForecastController>>().Object;

            var tracerProvider = new Mock<TracerProvider>().Object;

            var serviceBus = new Mock<ServiceBusClient>().Object;

            var configuration = new Mock<IConfiguration>().Object;

            this.options = new DbContextOptionsBuilder<WeatherForecastDbContext>()
                .UseInMemoryDatabase(databaseName: $"WeatherForecastDb{Guid.NewGuid()}")
                .Options;

            var repository = new Mock<IWeatherForecastRepository>().Object;

            this.controller = new WeatherForecastController(logger, tracerProvider, serviceBus, configuration, repository);
        }

        /// <summary>
        /// Unit test of the <see cref="M:WeatherForecastController.GetWeatherForecastsAsync"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetWeatherForecasts()
        {
            var result = await this.controller!.GetWeatherForecastsAsync();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        /// <summary>
        /// Unit test of the <see cref="M:WeatherForecastController.GetWeatherForecastsAsync"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetWeatherForecastById()
        {
            var result = await this.controller!.GetWeatherForecastAsync(int.MaxValue);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));

            WeatherForecast forecast = new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                TemperatureC = 25,
                Summary = "Warm"
            };

            result = await this.controller!.PutWeatherForecastAsync(forecast);

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));

            // Assert.AreNotEqual<int>(forecast.Id, 0);

            // Assert.IsInstanceOfType<WeatherForecast>(result.Value);

            // result = await this.controller!.GetWeatherForecastAsync(result.Value.Id);

            // Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        /// <summary>
        /// Unit test of the <see cref="M:WeatherForecastController.GetErrorAsync"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetErrorAsync()
        {
            var memoryCache = new Mock<IMemoryCache>().Object;

            var logger = new Mock<ILogger<WeatherForecastController>>().Object;

            var tracerProvider = new Mock<TracerProvider>().Object;

            var serviceBus = new Mock<ServiceBusClient>().Object;

            var configuration = new Mock<IConfiguration>().Object;

            var repository = new Mock<IWeatherForecastRepository>().Object;

            WeatherForecastController controller = new WeatherForecastController(logger, tracerProvider, serviceBus, configuration, repository);

            var ex = await Assert.ThrowsExceptionAsync<Exception>(controller.GetErrorAsync);

            Assert.AreEqual("Testing error routing.", ex.Message);
        }
    }
}