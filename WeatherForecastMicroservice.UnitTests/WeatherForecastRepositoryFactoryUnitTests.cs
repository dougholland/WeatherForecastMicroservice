namespace WeatherForecastMicroservice.UnitTests
{
    using WeatherForecastMicroservice.Entities;

    /// <summary>
    /// Provides unit tests for the <see cref="T:WeatherForecastRepositoryFactory"/> class.
    /// </summary>
    [TestClass]
    public class WeatherForecastRepositoryFactoryUnitTests
    {
        /// <summary>
        /// Tests the <see cref="M:WeatherForecastRepositoryFactory.CreateRepository"/> method.
        /// </summary>
        [TestMethod]
        public void WeatherForecastDbContextFactory()
        {
            var factory = new WeatherForecastRepositoryFactory();

            var repository = factory.CreateRepository();

            Assert.IsNotNull(repository);
        }
    }
}
