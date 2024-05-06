namespace WeatherForecastMicroservice.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the weather forecast model.
    /// </summary>
    public class WeatherForecast
    {
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
