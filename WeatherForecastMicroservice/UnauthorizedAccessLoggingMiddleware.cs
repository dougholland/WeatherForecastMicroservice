namespace WeatherForecastMicroservice
{
    using System.Diagnostics;

    /// <summary>
    /// Custom ASP.NET Core middleware that logs unauthorized access attempts.
    /// </summary>
    public class UnauthorizedAccessLoggingMiddleware
    {
        /// <summary>
        /// Represents a function that can process an HTTP request.
        /// </summary>
        private readonly RequestDelegate request;

        /// <summary>
        /// Creates an instance of the <see cref="T:UnauthorizedAccessLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="request">The <see cref="T:RequestDelegate"/> representing the middleware function that can process an HTTP request.</param>
        public UnauthorizedAccessLoggingMiddleware(RequestDelegate request)
        {
            this.request = request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            await this.request(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized || context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                var span = Activity.Current;

                span?.SetTag("unauthorized.access", true);
                span?.SetTag("path", context.Request.Path);
                span?.SetTag("method", context.Request.Method);
            }
        }
    }
}
