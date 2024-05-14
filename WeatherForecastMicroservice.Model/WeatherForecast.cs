namespace WeatherForecastMicroservice.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the weather forecast model.
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Creates a new instance of the <see cref="WeatherForecast"/> class.
        /// </summary>
        public WeatherForecast()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="WeatherForecast"/> class with properties copied from another instance.
        /// </summary>
        /// <param name="weatherForecast">The instance of the <see cref="WeatherForecast"/> class to copy.</param>
        public WeatherForecast(WeatherForecast weatherForecast)
        {
            if (weatherForecast == null)
            {
                throw new ArgumentNullException(nameof(weatherForecast), "Attempted to initialize a WeatherForecast object with a null reference.");
            }

            Id = weatherForecast.Id;

            Date = weatherForecast.Date;

            TemperatureC = weatherForecast.TemperatureC;

            Summary = weatherForecast.Summary;
        }

        /// <summary>
        /// Gets or sets the identifier weather forecast.
        /// </summary>
        /// <value>The identifier of the weather forecast.</value>
        [Key]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date of the weather forecast.
        /// </summary>
        /// <value>The date of the weather forecast.</value>
        [Required]
        public DateOnly Date
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the temperature in Celsius.
        /// </summary>
        /// <value>The temperature in Celsius.</value>
        public int TemperatureC
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the temperature in fahrenheit.
        /// </summary>
        /// <value>The temperature in fahrenheit.</value>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Gets or sets the summary of the weather forecast.
        /// </summary>
        /// <value>The summary of the weather forecast.</value>
        [Required]
        [MaxLength(50)]
        public string? Summary
        {
            get;
            set;
        }
    }
}
