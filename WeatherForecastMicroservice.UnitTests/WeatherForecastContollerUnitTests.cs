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

    /// <summary>
    /// Provides unit tests for the <see cref="WeatherForecastController"/> class.
    /// </summary>
    [TestClass]
    public class WeatherForecastContollerUnitTests
    {
        /// <summary>
        /// Unit test of the <see cref="WeatherForecastController.GetWeatherForecastsAsync"/> method.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetWeatherForecasts()
        {
            var memoryCache = new Mock<IMemoryCache>().Object;

            var logger = new Mock<ILogger<WeatherForecastController>>().Object;

            var tracerProvider = new Mock<TracerProvider>().Object;

            var serviceBus = new Mock<ServiceBusClient>().Object;

            var configuration = new Mock<IConfiguration>().Object;

            WeatherForecastController controller = new WeatherForecastController(memoryCache, logger, tracerProvider, serviceBus, configuration);

            var result = await controller.GetWeatherForecastsAsync();

            Assert.IsNotInstanceOfType(result, typeof(NotFoundResult));

            Assert.AreEqual<int>(5, result.Value?.Count());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetError()
        {
            var memoryCache = new Mock<IMemoryCache>().Object;

            var logger = new Mock<ILogger<WeatherForecastController>>().Object;

            var tracerProvider = new Mock<TracerProvider>().Object;

            var serviceBus = new Mock<ServiceBusClient>().Object;

            var configuration = new Mock<IConfiguration>().Object;

            WeatherForecastController controller = new WeatherForecastController(memoryCache, logger, tracerProvider, serviceBus, configuration);

            var ex = await Assert.ThrowsExceptionAsync<Exception>(controller.GetErrorAsync);

            Assert.AreEqual("Testing error routing.", ex.Message);
        }
    }
}