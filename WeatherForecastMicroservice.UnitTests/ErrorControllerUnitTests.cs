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
        private readonly Mock<ILogger<ErrorController>> logger = new Mock<ILogger<ErrorController>>();

        /// <summary>
        /// The mock environment to be used for unit tests.
        /// </summary>
        private readonly Mock<IHostEnvironment> environment = new Mock<IHostEnvironment>();

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
            var controller = new ErrorController(this.logger.Object);

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
            var controller = new ErrorController(this.logger.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = this.httpContext ?? new DefaultHttpContext()
            };

            var result = controller.HandleErrorDevelopment(environment.Object);

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Tests the <see cref="M:ErrorController.HandleErrorDevelopment"/> method, mocking its use within a production environment.
        /// </summary>
        [TestMethod]
        public void HandleErrorDevelopment_EnvironmentIsNotDevelopment()
        {
            var controller = new ErrorController(this.logger.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = this.httpContext ?? new DefaultHttpContext()
            };

            environment.Setup(e => e.EnvironmentName).Returns("Production");

            var result = controller.HandleErrorDevelopment(environment.Object);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
