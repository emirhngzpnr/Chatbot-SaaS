using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotSaaS.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; } = default!;

        public MessageRole Role { get; set; }
        public string Text { get; set; } = default!;
        public int? TokensIn { get; set; }
        public int? TokensOut { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
