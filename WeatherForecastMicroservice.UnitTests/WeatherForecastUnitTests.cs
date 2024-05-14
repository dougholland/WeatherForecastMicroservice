namespace WeatherForecastMicroservice.UnitTests
{
    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// Provides unit tests for the <see cref="WeatherForecast"/> model class.
    /// </summary>
    [TestClass]
    public class WeatherForecastUnitTests
    {
        /// <summary>
        /// Tests the constructors of the <see cref="WeatherForecast"/> class.
        /// </summary>
        [TestMethod]
        public void WeatherForecastConstuctors()
        {
            WeatherForecast forecast = new WeatherForecast();

            forecast.Id = 1;

            forecast.Date = DateOnly.FromDateTime(DateTime.Now);

            forecast.TemperatureC = 32;

            forecast.Summary = "Warm";

            Assert.AreEqual<int>(forecast.Id, 1);

            Assert.AreEqual<DateOnly>(forecast.Date, DateOnly.FromDateTime(DateTime.Now));

            Assert.AreEqual<int>(forecast.TemperatureC, 32);

            Assert.AreEqual<string>(forecast.Summary, "Warm");

            WeatherForecast copy = new WeatherForecast(forecast);

            Assert.AreEqual<int>(copy.Id, 1);

            Assert.AreEqual<DateOnly>(copy.Date, DateOnly.FromDateTime(DateTime.Now));

            Assert.AreEqual<int>(copy.TemperatureC, 32);

            Assert.AreEqual<string>(copy.Summary, "Warm");

            Assert.ThrowsException<ArgumentNullException>(() => new WeatherForecast(null));
        }
    }
}
