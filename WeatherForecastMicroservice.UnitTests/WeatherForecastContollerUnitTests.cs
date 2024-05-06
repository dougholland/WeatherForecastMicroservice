namespace WeatherForecastMicroservice.UnitTests
{
    using Azure.Messaging.ServiceBus;

    using Microsoft.AspNetCore.Mvc;
    
    using Microsoft.Extensions.Caching.Memory;

    using Microsoft.Extensions.Configuration;

    using Microsoft.Extensions.Logging;

    using Moq;

    using OpenTelemetry.Trace;

    using WeatherForecastMicroservice.Controllers;

    using WeatherForecastMicroservice.Entities;

    /// <summary>
    /// Provides unit tests for the <see cref="WeatherForecastController"/> class.
    /// </summary>
    [TestClass]
    public class WeatherForecastContollerUnitTests
    {
        /// <summary>
        /// Unit test of the <see cref="M:WeatherForecastController.GetWeatherForecastsAsync"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetWeatherForecasts()
        {
            var memoryCache = new Mock<IMemoryCache>().Object;

            var logger = new Mock<ILogger<WeatherForecastController>>().Object;

            var tracerProvider = new Mock<TracerProvider>().Object;

            var serviceBus = new Mock<ServiceBusClient>().Object;

            var configuration = new Mock<IConfiguration>().Object;

            var repository = new Mock<IWeatherForecastRepository>().Object;

            WeatherForecastController controller = new WeatherForecastController(logger, tracerProvider, serviceBus, configuration, repository);

            var result = await controller.GetWeatherForecastsAsync();

            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));

            Assert.AreEqual<int>(5, result.Value?.Count());
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