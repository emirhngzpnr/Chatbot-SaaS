using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotSaaS.Domain.Entities
{
    public class Bot
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; } = default!;

        public string BotKey { get; set; } = default!; // unique per tenant
        public string Name { get; set; } = default!;

        // İşletme özelleştirmesi
        public string SystemPrompt { get; set; } = "You are a helpful assistant.";
        public string Model { get; set; } = "gpt-4.1-mini"; // şimdilik varsayım
        public double Temperature { get; set; } = 0.2;
        public int MaxOutputTokens { get; set; } = 600;

        public bool IsActive { get; set; } = true;
        public List<Conversation> Conversations { get; set; } = new();
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
