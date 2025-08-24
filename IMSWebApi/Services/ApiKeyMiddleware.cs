namespace IMSWebApi.Services
{
    public class ApiKeyMiddleware
    {

        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-API-KEY";
        private const string API_KEY = "Ym11LnByb2plY3Q"; // Replace with your actual API key

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey(API_KEY_HEADER))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("API Key is missing.");
                return;
            }

            var apiKey = httpContext.Request.Headers[API_KEY_HEADER].ToString();

            // Check if the provided API key matches
            if (apiKey != API_KEY)
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await httpContext.Response.WriteAsync("Invalid API Key.");
                return;
            }

            // Proceed with the next middleware
            await _next(httpContext);
        }

    }
}
