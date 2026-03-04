using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotSaaS.Application.DTOs.Chat
{
    public record ConversationDto(Guid Id, string SessionId, DateTime CreatedAtUtc, DateTime LastActiveAtUtc, List<MessageDto> Messages);
}
