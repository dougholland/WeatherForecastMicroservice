namespace WeatherForecastMicroservice.UnitTests
{
    using WeatherForecastMicroservice.Controllers;

    using Moq;

    using Microsoft.Extensions.Logging;

    using Microsoft.AspNetCore.Http;

    using Microsoft.AspNetCore.Mvc;

    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Provides unit tests for the <see cref="T:ErrorController"/> class.
    /// </summary>
    [TestClass]
    public class ErrorControllerUnitTests
    {
        /// <summary>
        /// The HttpContext instance to be used for unit tests.
        /// </summary>
        private DefaultHttpContext? httpContext;

        /// <summary>
        /// The mock logger instance to be used for unit tests.
        /// </summary>
        private readonly ILogger<ErrorController> logger = new Mock<ILogger<ErrorController>>().Object;

        /// <summary>
        /// The mock environment to be used for unit tests.
        /// </summary>
        private readonly IHostEnvironment environment = new Mock<IHostEnvironment>().Object;

        /// <summary>
        /// Provides initialization for unit tests.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.httpContext = new DefaultHttpContext();
        }

        /// <summary>
        /// Tests the <see cref="M:ErrorController.HandleError"/> method.
        /// </summary>
        [TestMethod]
        public void HandleError()
        {
            var controller = new ErrorController(this.logger);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = this.httpContext ?? new DefaultHttpContext()
            };

            var result = controller.HandleError();

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the <see cref="M:ErrorController.HandleErrorDevelopment"/> method.
        /// </summary>
        [TestMethod]
        public void HandleErrorDevelopment()
        {
            var controller = new ErrorController(this.logger);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = this.httpContext ?? new DefaultHttpContext()
            };

            var result = controller.HandleErrorDevelopment(environment);

            Assert.IsNotNull(result);
        }
    }
}
