namespace WeatherForecastMicroservice.Entities
{
    using Microsoft.EntityFrameworkCore;

    using Microsoft.EntityFrameworkCore.Design;

    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Creates instances of the <see cref="T:WeatherForecastDbContext"/> class.
    /// </summary>
    public class WeatherForecastDbContextFactory : IDesignTimeDbContextFactory<WeatherForecastDbContext>
    {
        /// <summary>
        /// Creates an instance of the <see cref="T:WeatherForecastDbContext"/> class.
        /// </summary>
        /// <param name="args">An array of argumments.</param>
        /// <returns>An instance of the <see cref="T:WeatherForecastDbContext"/> class.</returns>
        public WeatherForecastDbContext CreateDbContext(string[] args)
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
    }
}
