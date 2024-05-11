namespace WeatherForecastMicroservice.Entities
{
    public class WeatherForecastRepositoryFactory
    {
        public WeatherForecastRepositoryFactory()
        {
        }

        public IWeatherForecastRepository CreateRepository()
        {
            WeatherForecastDbContext context = new WeatherForecastDbContextFactory().CreateDbContext();

            return new WeatherForecastRepository(context) as IWeatherForecastRepository;
        }
    }
}
