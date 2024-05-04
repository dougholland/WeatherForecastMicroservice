namespace WeatherForecastMicroservice.Entities
{
    using WeatherForecastMicroservice.Model;

    /// <summary>
    /// Represents a repository for weather forecasts.
    /// </summary>
    public interface IWeatherForecastRepository
    {
        /// <summary>
        /// Gets all weather forecasts asynchronously.
        /// </summary>
        /// <returns>All weather forecasts.</returns>
        Task<IEnumerable<WeatherForecast>> GetAllForecastsAsync();

        /// <summary>
        /// Gets a weather forecast asynchronously.
        /// </summary>
        /// <param name="id">The identifier of the weather forecast.</param>
        /// <returns>The weather forecast.</returns>
        Task<WeatherForecast?> GetForecastByIdAsync(int id);

        /// <summary>
        /// Saves the specified weather forecast to the repository.
        /// </summary>
        /// <param name="forecast">The weather forecast to be saved within the repository.</param>
        /// <returns>The number of </returns>
        Task<int> SaveForecastAsync(WeatherForecast forecast);

        /// <summary>
        /// Deletes the weather forecast represented by the identifier from the repository.
        /// </summary>
        /// <param name="id">The identifier of the weather forecast.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains the number of state entries deleted from the database.</returns>
        Task<int> DeleteForecastByIdAsync(int id);

        /// <summary>
        /// Deletes the weather forecast from the repository.
        /// </summary>
        /// <param name="forecast">The weather forecast to be deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result contains the number of state entries deleted from the database.</returns>
        Task<int> DeleteForecastAsync(WeatherForecast forecast);

        /// <summary>
        /// Updates the weather forecast within the repository.
        /// </summary>
        /// <param name="forecast">The weather forecast to be updated.</param>
        /// <returns>A task that represents the asynchronous update operation. The task result contains the number of state entries updated within the database.</returns>
        Task<int> UpdateForecastAsync(WeatherForecast forecast);
    }
}
