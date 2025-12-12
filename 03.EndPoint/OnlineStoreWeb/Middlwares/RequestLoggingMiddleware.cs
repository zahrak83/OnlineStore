namespace OnlineStoreWeb.Middlwares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;


        public RequestLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;
            await LogRequest(context, requestId);
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await _next(context);
                stopwatch.Stop();

                // Log response
                await LogResponse(context, requestId, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(
                    ex,
                    "Request {RequestId} failed after {ElapsedMs}ms",
                    requestId,
                    stopwatch.ElapsedMilliseconds
                );
                throw;
            }
            finally
            {
                // Copy response body back to original stream
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }


        private async Task LogRequest(HttpContext context, string requestId)
        {
            var request = context.Request;

            // Read request body
            request.EnableBuffering();
            var body = await ReadStreamAsync(request.Body);
            request.Body.Position = 0;

            _logger.LogInformation(
                "Request {RequestId}: {Method} {Path} {QueryString} from {RemoteIp}",
                requestId,
                request.Method,
                request.Path,
                request.QueryString,
                context.Connection.RemoteIpAddress
            );

            if (!string.IsNullOrEmpty(body))
            {
                _logger.LogDebug(
                    "Request {RequestId} Body: {Body}",
                    requestId,
                    body
                );
            }
        }

        private async Task LogResponse(
            HttpContext context,
            string requestId,
            long elapsedMs)
        {
            var response = context.Response;

            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await ReadStreamAsync(response.Body);
            response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(
                "Response {RequestId}: {StatusCode} in {ElapsedMs}ms",
                requestId,
                response.StatusCode,
                elapsedMs
            );

            if (!string.IsNullOrEmpty(body) && response.StatusCode >= 400)
            {
                _logger.LogWarning(
                    "Response {RequestId} Error Body: {Body}",
                    requestId,
                    body
                );
            }
        }

        private static async Task<string> ReadStreamAsync(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream, leaveOpen: true);
            var content = await reader.ReadToEndAsync();
            stream.Seek(0, SeekOrigin.Begin);
            return content;
        }

    }
}
