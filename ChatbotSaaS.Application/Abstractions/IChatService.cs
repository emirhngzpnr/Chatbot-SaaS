using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatbotSaaS.Application.DTOs.Chat;

namespace ChatbotSaaS.Application.Abstractions
{
    public interface IChatService
    {
        Task<MessageDto> SendMessageAsync(string tenantKey, string botKey, string sessionId, SendMessageRequest request, CancellationToken ct);
        Task<ConversationDto> GetConversationAsync(string tenantKey, string botKey, string sessionId, int takeLast, CancellationToken ct);

    }
}
