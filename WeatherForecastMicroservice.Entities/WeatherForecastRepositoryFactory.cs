namespace WeatherForecastMicroservice.Entities
{
    public class WeatherForecastRepositoryFactory
    {
        public WeatherForecastRepositoryFactory()
        {
        }

        public IWeatherForecastRepository CreateRepository()
        {
            return new WeatherForecastRepository() as IWeatherForecastRepository;
        }
    }
}
