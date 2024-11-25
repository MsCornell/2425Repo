using System.Text.Json;
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

        // { "boardState": "XOXO-OX-X"}
        [Function("NextMove")]
        public async Task<IActionResult> Run2([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            try
            {
                var requestBody = await JsonSerializer.DeserializeAsync<JsonElement>(req.Body);

                if (requestBody.ValueKind == JsonValueKind.Undefined || requestBody.ValueKind == JsonValueKind.Null)
                {
                    return new BadRequestObjectResult("Payload JSON was invalid.");
                }

                if (!requestBody.TryGetProperty("boardState", out var boardStateElement))
                {
                    return new BadRequestObjectResult("Payload JSON does not include boardState property.");
                }

                if (boardStateElement.GetString() is not { Length: 9 } boardState)
                {
                    return new BadRequestObjectResult("Payload JSON boardState property is not 9 chars.");
                }

                var boardArray = boardState.ToCharArray();

                var nextIndex = boardArray
                    .Select((value, index) => new { value, index })
                    .Where(x => x.value == '-')
                    .Select(x => x.index + 1)
                    .OrderBy(_ => Guid.NewGuid())
                    .FirstOrDefault();

                return new OkObjectResult($"{{ 'nextMove': {nextIndex} }}");
            }
            catch
            {
                return new BadRequestObjectResult("Invalid JSON payload.");
            }
        }
    }
}