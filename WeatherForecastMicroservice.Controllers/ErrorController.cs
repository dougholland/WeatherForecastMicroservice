namespace WeatherForecastMicroservice.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.AspNetCore.Diagnostics;
    
    using Microsoft.Extensions.Logging;

    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Error controller.
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// An instance of the <see cref="T:ILogger"/> interface to be used for logging.
        /// </summary>
        private readonly ILogger<ErrorController> logger;

        /// <summary>
        /// Creates a new instance of the <see cref="T:ErrorController"/> class. 
        /// </summary>
        /// <param name="logger">The instance of the <see cref="T:ILogger"/> interface to be used for logging.</param>
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Handles an error within a development environment, that occurs within the Weather Forecast Microservice.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/> representing the result of the action method.</returns>
        [Route("/error-development")]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment environment)
        {
            if ( !environment.IsDevelopment())
            {
                return NotFound();    
            }

            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            this.logger.LogError(feature?.Error, feature?.Error.Message);

            return Problem(detail: feature?.Error.StackTrace, title: feature?.Error.Message);
        }

        /// <summary>
        /// Handles an error outside of the development environment, that occurs within the Weather Forecast Microservice.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Mvc.IActionResult"/> representing the result of the action method.</returns>
        [Route("/error")]
        [HttpGet]
        public IActionResult HandleError()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            
            this.logger.LogError(feature?.Error, feature?.Error.Message);

            return Problem();
        }
    }
}
