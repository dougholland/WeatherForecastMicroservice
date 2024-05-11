namespace WeatherForecastMicroservice.Entities
{
    using Microsoft.EntityFrameworkCore;

    using Microsoft.EntityFrameworkCore.Design;

    using Microsoft.Extensions.Configuration;
    using System.Diagnostics;

    /// <summary>
    /// Creates instances of the <see cref="T:WeatherForecastDbContext"/> class.
    /// </summary>
    public class WeatherForecastDbContextFactory : IDesignTimeDbContextFactory<WeatherForecastDbContext>
    {
        /// <summary>
        /// The name of the GitHub Actions environment variable that contains the connection string.
        /// </summary>
        private const string EnvironmentVariable = "DB_CONNECTION_STRING";

        /// <summary>
        /// Creates an instance of the <see cref="T:WeatherForecastDbContext"/> class.
        /// </summary>
        /// <returns>An instance of the <see cref="T:WeatherForecastDbContext"/> class.</returns>
        public WeatherForecastDbContext CreateDbContext()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<WeatherForecastDbContext>();

            string connectionString = configuration["AzureSqlConnection"] ?? string.Empty;

            builder.UseSqlServer(connectionString);

            return new WeatherForecastDbContext(builder.Options);
        }

        /// <summary>
        /// Creates an instance of the <see cref="T:WeatherForecastDbContext"/> class.
        /// </summary>
        /// <remarks>
        /// This method is used by the Entity Framework Core tools to create an instance of the <see cref="T:WeatherForecastDbContext"/> class.
        /// </remarks>
        /// <param name="args">An array of argumments.</param>
        /// <returns>An instance of the <see cref="T:WeatherForecastDbContext"/> class.</returns>
        public WeatherForecastDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<WeatherForecastDbContext>();

            // Get the connection string from the GitHub actions environment variable.
            string connectionString = Environment.GetEnvironmentVariable(EnvironmentVariable) ?? string.Empty;
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception(string.Format("GitHub Actions environment variable '{0}' not found.", EnvironmentVariable));
            }
            else
            {
                builder.UseSqlServer(connectionString);
            }

            return new WeatherForecastDbContext(builder.Options);
        }
    }
}
