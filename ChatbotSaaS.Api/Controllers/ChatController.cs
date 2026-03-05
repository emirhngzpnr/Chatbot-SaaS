using ChatbotSaaS.Application.Abstractions;
using ChatbotSaaS.Application.DTOs.Chat;
using Microsoft.AspNetCore.Mvc;

namespace ChatbotSaaS.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("{tenantKey}/{botKey}/{sessionId}")]
        public async Task<ActionResult<MessageDto>> SendMessage(
            string tenantKey,
            string botKey,
            string sessionId,
            [FromBody] SendMessageRequest request,
            CancellationToken ct)
        {
            var result = await _chatService.SendMessageAsync(
                tenantKey,
                botKey,
                sessionId,
                request,
                ct);

            return Ok(result);
        }

        [HttpGet("{tenantKey}/{botKey}/{sessionId}")]
        public async Task<ActionResult<ConversationDto>> GetConversation(
            string tenantKey,
            string botKey,
            string sessionId,
            int takeLast = 50,
            CancellationToken ct = default)
        {
            var result = await _chatService.GetConversationAsync(
                tenantKey,
                botKey,
                sessionId,
                takeLast,
                ct);

            return Ok(result);
        }
    }
}
