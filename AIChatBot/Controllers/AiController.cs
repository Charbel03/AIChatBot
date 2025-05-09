using AIChatBot.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIChatBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("Message")]
        public async Task<IActionResult> GetAiMessage([FromBody] string question)
        {
            var result = await _aiService.StartChat(question);
            return Ok(result);
        }

        [HttpPost("ResetChatMessages")]
        public async Task<IActionResult> ClearAiMessage()
        {
            _aiService.resetChatMessages();
            return Ok();
        }
    }
}
