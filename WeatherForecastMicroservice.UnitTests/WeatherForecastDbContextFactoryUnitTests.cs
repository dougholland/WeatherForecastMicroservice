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
        /// The name of the GitHub Actions environment variable that contains the connection string.
        /// </summary>
        private const string EnvironmentVariable = "DB_CONNECTION_STRING";

        /// <summary>
        /// Tests the <see cref="M:WeatherForecastDbContextFactory.CreateDbContext"/> method.
        /// </summary>
        [TestMethod]
        public void WeatherForecastDbContextFactory()
        {
            var factory = new WeatherForecastDbContextFactory();

            var context1 = factory.CreateDbContext();

            Assert.IsNotNull(context1);

            Assert.IsInstanceOfType<WeatherForecastDbContext>(context1);

            Environment.SetEnvironmentVariable(EnvironmentVariable, "Server=tcp:unittest-server.database.windows.net,1433;Initial Catalog=weatherforecastmicroservice-database;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";");

            var context2 = factory.CreateDbContext(new string[] { });

            Assert.IsNotNull(context2);

            Assert.IsInstanceOfType<WeatherForecastDbContext>(context2);

            Environment.SetEnvironmentVariable(EnvironmentVariable, string.Empty);

            Assert.ThrowsException<System.InvalidOperationException>(() => factory.CreateDbContext(new string[] { }));
        }
    }
}
