namespace WeatherForecastMicroservice.Entities
{
    /// <summary>
    /// Creates instances of the <see cref="T:WeatherForecastRepository"/> class. 
    /// </summary>
    public class WeatherForecastRepositoryFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="T:WeatherForecastRepositoryFactory"/> class.
        /// </summary>
        public WeatherForecastRepositoryFactory()
        {
        }

        /// <summary>
        /// Creates an instance of the <see cref="T:WeatherForecastRepository"/> class.
        /// </summary>
        /// <returns>An instance of the <see cref="T:WeatherForecastRepository"/> class.</returns>
        public IWeatherForecastRepository CreateRepository()
        {
            WeatherForecastDbContext context = new WeatherForecastDbContextFactory().CreateDbContext();

            return new WeatherForecastRepository(context) as IWeatherForecastRepository;
        }
    }
}
