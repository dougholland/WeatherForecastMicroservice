namespace WeatherForecastMicroservice.Entities
{
    using Microsoft.EntityFrameworkCore;

    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// The weather forecast database context.
    /// </summary>
    public class WeatherForecastDbContext : DbContext
    {
        /// <summary>
        /// Creates an instance of the <see cref="T:WeatherForecastDbContex"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the weather forecasts.
        /// </summary>
        public DbSet<WeatherForecast> WeatherForecasts
        {
            get;
            set;
        }
    }
}
