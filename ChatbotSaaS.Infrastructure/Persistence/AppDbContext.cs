using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatbotSaaS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatbotSaaS.Infrastructure.Persistence
{
    public  class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<Bot> Bots => Set<Bot>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tenant unique key
            modelBuilder.Entity<Tenant>()
                .HasIndex(x => x.TenantKey)
                .IsUnique();

            // Bot unique key per tenant
            modelBuilder.Entity<Bot>()
                .HasIndex(x => new { x.TenantId, x.BotKey })
                .IsUnique();

            // Conversation session
            modelBuilder.Entity<Conversation>()
                .HasIndex(x => new { x.TenantId, x.BotId, x.SessionId })
                .IsUnique();

            // Temperature precision
            modelBuilder.Entity<Bot>()
                .Property(x => x.Temperature)
                .HasPrecision(3, 2);

            // Tenant → Bots
            modelBuilder.Entity<Bot>()
                .HasOne(b => b.Tenant)
                .WithMany(t => t.Bots)
                .HasForeignKey(b => b.TenantId);

            // Bot → Conversations
            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Bot  )
                .WithMany(b => b.Conversations)
                .HasForeignKey(c => c.BotId);

            // Conversation → Messages
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId);

            base.OnModelCreating(modelBuilder);
        }


    }
}
