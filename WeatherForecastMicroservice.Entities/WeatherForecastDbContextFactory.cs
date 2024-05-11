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

            string connectionString = Environment.GetEnvironmentVariable("AzureSqlConnection") ?? string.Empty;
            
            if (string.IsNullOrEmpty(connectionString))
            {
                Trace.WriteLine("AzureSqlConnection environment variable not found.");
            }
            else
            {
                builder.UseSqlServer(connectionString);
            }

            return new WeatherForecastDbContext(builder.Options);
        }
    }
}
