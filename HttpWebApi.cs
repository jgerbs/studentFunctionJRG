using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Games.Function
{
    public class HttpWebApi
    {
        private readonly ILogger<HttpWebApi> _logger;

        public HttpWebApi(ILogger<HttpWebApi> logger)
        {
            _logger = logger;
        }

        [Function("HttpWebApi")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
