// Controllers/RabbitMQController.cs

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MQTTBackend.Services;

namespace MQTTBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        private readonly RabbitMQService _rabbitMQService;

        public RabbitMQController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }
        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok(new
            {
                Status = "Good"
            });
        }

        [HttpPost("send")]
        public IActionResult Send([FromBody] JsonElement message)
        {
            try
            {
                var jsonString = message.GetRawText();

                using (JsonDocument doc = JsonDocument.Parse(jsonString))
                {
                    if (doc.RootElement.TryGetProperty("routeKey", out JsonElement routeKeyElement))
                    {
                        string routeKey = routeKeyElement.GetString();
                        string orderJson = _rabbitMQService.CreateOrderJson(routeKey);
                        var orderJsonObject = JsonSerializer.Deserialize<JsonElement>(orderJson);
                        _rabbitMQService.SendMessage(orderJson);

                        return Ok(new
                        {
                            Status = "Message sent",
                            SentJson = orderJsonObject
                        });
                    }
                    else
                    {
                        return BadRequest("routeKey not found in the message");
                    }
                }

            }
            catch (Exception ex)
            {
                return BadRequest($@"Message is not in a vaild JSON format: {ex.Message}");
            }
        }
    }
}