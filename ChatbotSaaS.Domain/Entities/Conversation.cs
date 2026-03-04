using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotSaaS.Domain.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = default!;

        public Guid BotId { get; set; }
        public Bot Bot { get; set; } = default!;

        public string SessionId { get; set; } = default!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastActiveAtUtc { get; set; } = DateTime.UtcNow;

        public List<Message> Messages { get; set; } = new();
    }
}
