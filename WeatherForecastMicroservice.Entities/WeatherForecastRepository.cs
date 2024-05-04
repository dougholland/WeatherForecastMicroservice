namespace WeatherForecastMicroservice.Entities
{
    using Microsoft.EntityFrameworkCore;

    using WeatherForecastMicroservice.Entities;

    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// 
    /// </summary>
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private WeatherForecastDbContext context;

        /// <summary>
        /// Creates an instance of the <see cref="T:WeatherForecastRepository"/> class. 
        /// </summary>
        public WeatherForecastRepository()
        {
            WeatherForecastDbContextFactory factory = new WeatherForecastDbContextFactory();

            this.context = factory.CreateDbContext(new string[] { });
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
        /// <returns>All weather forecasts.</returns>
        public async Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync()
        {
            return await this.context.WeatherForecasts.ToListAsync();
        }

        /// <summary>
        /// Gets a weather forecast asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the weather forecast.</param>
        /// <returns>The weather forecast.</returns>
        public async Task<WeatherForecast?> GetForecastByIdAsync(int id)
        {
            return await this.context.WeatherForecasts.FindAsync(id);
        }

        /// <summary>
        /// Adds the specified weather forecast to the repository.
        /// </summary>
        /// <param name="forecast">The weather forecast to be added to the repository.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        public async Task<int> SaveForecastAsync(WeatherForecast forecast)
        {
            context.WeatherForecasts.Add(forecast);

            return await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the weather forecast represented by the identifier from the repository.
        /// </summary>
        /// <param name="id">The identifier of the weather forecast.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains the number of state entries deleted from the database.</returns>
        public async Task<int> DeleteForecastByIdAsync(int id)
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
        public async Task<int> DeleteForecastAsync(WeatherForecast forecast)
        {
            return await DeleteForecastByIdAsync(forecast.Id);
        }

        /// <summary>
        /// Updates the weather forecast within the repository.
        /// </summary>
        /// <param name="forecast">The weather forecast to be updated.</param>
        /// <returns>A task that represents the asynchronous update operation. The task result contains the number of state entries updated within the database.</returns>
        public async Task<int> UpdateForecastAsync(WeatherForecast forecast)
        {
            this.context.Entry<WeatherForecast>(forecast).State = EntityState.Modified;

            return await this.context.SaveChangesAsync();
        }
    }
}
