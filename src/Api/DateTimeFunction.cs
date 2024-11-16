using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class DateTimeFunction
    {
        private readonly ILogger<DateTimeFunction> _logger;

        public DateTimeFunction(ILogger<DateTimeFunction> logger)
        {
            _logger = logger;
        }

        [Function("Now")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            return new OkObjectResult($"The current UTC date time is {DateTime.UtcNow}");
        }
    }
}
