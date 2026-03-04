using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatbotSaaS.Domain.Entities
{
    public class Tenant
    {
        public Guid Id { get; set; } = Guid.NewGuid();

       
        public string TenantKey { get; set; } = default!; // unique

        public string Name { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public List<Bot> Bots { get; set; } = new();
    }
}
