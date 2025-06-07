namespace SrManoelLoja.Filters
{
    public class ApiLoggingFilter : IEndpointFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger) => _logger = logger;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            _logger.LogInformation("### Executando");
            _logger.LogInformation("####################################");
            _logger.LogInformation($"Horário: {DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"Caminho da Requisição: {context.HttpContext.Request.Path}");
            _logger.LogInformation("####################################");

            var result = await next(context);

            _logger.LogInformation("### Executado");
            _logger.LogInformation("####################################");
            _logger.LogInformation($"Horário: {DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"Status Code Final: {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("####################################");

            return result;
        }
    }
}