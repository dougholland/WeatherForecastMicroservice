namespace WeatherForecastMicroservice.Entities
{
    using Microsoft.EntityFrameworkCore;

    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// 
    /// </summary>
    public class WeatherForecastDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<WeatherForecast> WeatherForecasts
        {
            get;
            set;
        }
    }
}
