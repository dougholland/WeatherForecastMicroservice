namespace WeatherForecastMicroservice.Entities
{
    using Microsoft.EntityFrameworkCore;

    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// Weather forecast repository.
    /// </summary>
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        /// <summary>
        /// The weather forecast database context.
        /// </summary>
        private WeatherForecastDbContext context;

        /// <summary>
        /// Creates an instance of the <see cref="T:WeatherForecastRepository"/> class. 
        /// </summary>
        public WeatherForecastRepository()
        {
            WeatherForecastDbContextFactory factory = new WeatherForecastDbContextFactory();

            this.context = factory.CreateDbContext();
        }

        /// <summary>
        /// Creates an instance of the <see cref="T:WeatherForecastRepository"/> class. 
        /// </summary>
        /// <param name="context">An instance of the <see cref="T:WeatherForecastDbContext"/> to be used by the repository.</param>
        public WeatherForecastRepository(WeatherForecastDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets all weather forecasts asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation to get all forecasts. The task result contains all the weather forecasts within the repository.</returns>
        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastsAsync()
        {
            return await this.context.WeatherForecasts.ToListAsync();
        }

        /// <summary>
        /// Gets a weather forecast asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the weather forecast.</param>
        /// <returns>A task that represents the asynchronous operation to get a specific forecast represented by the identifier. The task result contains the forecast, if found within the repository.</returns>
        public async Task<WeatherForecast?> GetWeatherForecastByIdAsync(int id)
        {
            return await this.context.WeatherForecasts.FindAsync(id);
        }

        /// <summary>
        /// Adds the specified weather forecast to the repository.
        /// </summary>
        /// <param name="forecast">The weather forecast to be added to the repository.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        public async Task<WeatherForecast> AddWeatherForecastAsync(WeatherForecast forecast)
        {
            context.WeatherForecasts.Add(forecast);

            await context.SaveChangesAsync();

            return forecast;
        }

        /// <summary>
        /// Updates the weather forecast within the repository.
        /// </summary>
        /// <param name="forecast">The weather forecast to be updated.</param>
        /// <returns>A task that represents the asynchronous update operation. The task result contains the number of state entries updated within the database.</returns>
        public async Task<WeatherForecast> UpdateWeatherForecastAsync(WeatherForecast forecast)
        {
            this.context.Entry<WeatherForecast>(forecast).State = EntityState.Modified;

            await this.context.SaveChangesAsync();

            return forecast;
        }

        /// <summary>
        /// Deletes the weather forecast represented by the identifier from the repository.
        /// </summary>
        /// <param name="id">The identifier of the weather forecast.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains the number of state entries deleted from the database.</returns>
        public async Task<int> DeleteWeatherForecastByIdAsync(int id)
        {
            int deleted = 0;

            var forecast = await context.WeatherForecasts.FindAsync(id);

            if (forecast != null)
            {
                this.context.WeatherForecasts.Remove(forecast);

                deleted = await this.context.SaveChangesAsync();
            }

            return deleted;
        }

        /// <summary>
        /// Deletes the weather forecast from the repository.
        /// </summary>
        /// <param name="forecast">The weather forecast to be deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains the number of state entries deleted from the database.</returns>
        public async Task<int> DeleteWeatherForecastAsync(WeatherForecast forecast)
        {
            return await DeleteWeatherForecastByIdAsync(forecast.Id);
        }
    }
}
