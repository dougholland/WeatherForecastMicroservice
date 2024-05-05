namespace WeatherForecastMicroservice.UnitTests
{
    using Microsoft.Extensions.Configuration;

    using WeatherForecastMicroservice.Entities;

    /// <summary>
    /// Provides unit tests for the <see cref="T:WeatherForecastDbContextFactory"/> class.
    /// </summary>
    [TestClass]
    public class WeatherForecastDbContextFactoryUnitTests
    {
        /// <summary>
        /// Provides initialization for unit tests.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {            
        }

        /// <summary>
        /// Tests the <see cref="M:WeatherForecastDbContextFactory.CreateDbContext"/> method.
        /// </summary>
        [TestMethod]
        public void WeatherForecastDbContextFactory()
        {
            var factory = new WeatherForecastDbContextFactory();

            var context = factory.CreateDbContext();

            Assert.IsNotNull(context);
        }
    }
}
