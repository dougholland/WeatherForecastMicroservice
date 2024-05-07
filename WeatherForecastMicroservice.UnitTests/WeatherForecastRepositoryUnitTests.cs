namespace WeatherForecastMicroservice.UnitTests
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using WeatherForecastMicroservice.Entities;
    
    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// Provides unit tests of the <see cref="T:WeatherForecastRepository"/> class.
    /// </summary>
    [TestClass]
    public class WeatherForecastRepositoryUnitTests
    {
        /// <summary>
        /// 
        /// </summary>
        private DbContextOptions<WeatherForecastDbContext>? options;

        /// <summary>
        /// Provides initialization for unit tests.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.options = new DbContextOptionsBuilder<WeatherForecastDbContext>()
                .UseInMemoryDatabase(databaseName: $"WeatherForecastDb{Guid.NewGuid()}")
                .Options;
        }

        /// <summary>
        /// Tests the <see cref="M:WeatherForecastRepository.GetAllForecastsAsync"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetAllForecastsAsnc()
        {
            using var context = new WeatherForecastDbContext(options);

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            context.WeatherForecasts.AddRange(
                new WeatherForecast { Date = today, TemperatureC = 25, Summary = "Warm" },
                new WeatherForecast { Date = today, TemperatureC = 10, Summary = "Cold" }
            );

            context.SaveChanges();

            var repository = new WeatherForecastRepository(context);

            var result = await repository.GetWeatherForecastsAsync();

            Assert.AreEqual<int>(2, result.Count());
        }

        /// <summary>
        /// Tests the <see cref="M:WeatherForecastRepository.GetWeatherForecastAsync"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GetWeatherForecastAsync()
        {
            using var context = new WeatherForecastDbContext(options);

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            var forecast = new WeatherForecast { Date = today, TemperatureC = 25, Summary = "Warm" };
            context.Add<WeatherForecast>(forecast);

            context.SaveChanges();

            var repository = new WeatherForecastRepository(context);

            var result = await repository.GetWeatherForecastByIdAsync(forecast.Id);

            Assert.IsNotNull(result);

            Assert.AreEqual<DateOnly>(forecast.Date, result.Date);

            Assert.AreEqual<int>(forecast.TemperatureC, result.TemperatureC);

            Assert.AreEqual<int>(forecast.TemperatureF, result.TemperatureF);

            Assert.AreEqual<string>(forecast.Summary, result.Summary);
        }

        /// <summary>
        /// Tests the <see cref="M:WeatherForecastRepository.AddWeatherForecastAsync"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task AddWeatherForecast()
        {
            using var context = new WeatherForecastDbContext(options);

            var repository = new WeatherForecastRepository(context);

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            var forecast = new WeatherForecast { Date = today, TemperatureC = 25, Summary = "Warm" };

            await repository.AddWeatherForecastAsync(forecast);

            Assert.AreEqual<int>(1, context.WeatherForecasts.Count());

            Assert.IsTrue(context.WeatherForecasts.Any(forecast => forecast.Date == today));

            Assert.IsTrue(context.WeatherForecasts.Any(forecast => forecast.TemperatureC == 25));

            Assert.IsTrue(context.WeatherForecasts.Any(forecast => forecast.Summary == "Warm"));
        }

        /// <summary>
        /// Tests the <see cref="M:WeatherForecastRepository.UpdateWeatherForecastAsync"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task UpdateWeatherForecast()
        {
            using var context = new WeatherForecastDbContext(options);
           
            var repository = new WeatherForecastRepository(context);

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            var forecast = new WeatherForecast { Date = today, TemperatureC = 25, Summary = "Warm" };

            forecast = await repository.AddWeatherForecastAsync(forecast);

            forecast.TemperatureC = 30;

            await repository.UpdateWeatherForecastAsync(forecast);

            var updatedForecast = await context.WeatherForecasts.FindAsync(forecast.Id);

            Assert.AreEqual<int>(1, context.WeatherForecasts.Count());

            Assert.AreEqual<int>(forecast.Id, updatedForecast?.Id);

            Assert.AreEqual<DateOnly>(forecast.Date, updatedForecast?.Date);

            Assert.AreEqual<string>(forecast.Summary, updatedForecast?.Summary);

            Assert.AreEqual<int>(forecast.TemperatureC, updatedForecast?.TemperatureC);

            Assert.AreEqual<int>(forecast.TemperatureF, updatedForecast?.TemperatureF);
        }

        /// <summary>
        /// Tests the <see cref="M:WeatherForecastRepository.DeleteWeatherForecastAsync"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task DeleteWeatherForecast()
        {
            using var context = new WeatherForecastDbContext(options);

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            var forecast = new WeatherForecast { Date = today, TemperatureC = 25, Summary = "Warm" };

            context.Add(forecast);

            var saved = await context.SaveChangesAsync();

            Assert.AreEqual<int>(1, saved);

            var repository = new WeatherForecastRepository(context);

            var deleted = await repository.DeleteWeatherForecastAsync(forecast);

            Assert.AreEqual(1, deleted);

            var result = await context.WeatherForecasts.FindAsync(forecast?.Id);

            Assert.IsNull(result);
        }

        /// <summary>
        /// Tests the <see cref="M:WeatherForecastRepository.DeleteWeatherForecast"/> method.
        /// </summary>
        /// <returns>A task that represents the asynchronous unit test.</returns>
        [TestMethod]
        public async Task DeleteWeatherForecastById()
        {
            using var context = new WeatherForecastDbContext(options);

            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            var forecast = new WeatherForecast { Date = today, TemperatureC = 25, Summary = "Warm" };

            context.Add(forecast);

            var saved = await context.SaveChangesAsync();

            Assert.AreEqual<int>(1, saved);

            var repository = new WeatherForecastRepository(context);

            var deleted = await repository.DeleteWeatherForecastByIdAsync(forecast.Id);

            Assert.AreEqual(1, deleted);

            var result = await context.WeatherForecasts.FindAsync(forecast?.Id);

            Assert.IsNull(result);
        }
    }
}
