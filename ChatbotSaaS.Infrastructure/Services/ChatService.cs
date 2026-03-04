using System.Linq;
using ChatbotSaaS.Application.Abstractions;
using ChatbotSaaS.Application.DTOs.Chat;
using ChatbotSaaS.Domain.Entities;
using ChatbotSaaS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ChatbotSaaS.Infrastructure.Services;

public class ChatService : IChatService
{
    private readonly AppDbContext _db;
    public ChatService(AppDbContext db) => _db = db;

    public async Task<MessageDto> SendMessageAsync(
        string tenantKey,
        string botKey,
        string sessionId,
        SendMessageRequest request,
        CancellationToken ct)
    {

        var conversationId = await EnsureConversationAsync(tenantKey, botKey, sessionId, ct);

        var msg = new Message
        {
            ConversationId = conversationId,
            Role = request.Role,
            Text = request.Text,
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Messages.Add(msg);
        await _db.SaveChangesAsync(ct);

    
        return new MessageDto(msg.Id, msg.Role, msg.Text, msg.CreatedAtUtc);
    }

    public async Task<ConversationDto> GetConversationAsync(
        string tenantKey,
        string botKey,
        string sessionId,
        int takeLast,
        CancellationToken ct)
    {
        if (takeLast <= 0) takeLast = 50;
        if (takeLast > 200) takeLast = 200;

        // OPTİMİZASYON: Join kullanarak Tenant, Bot ve Conversation kontrolünü tek sorguda yapıyoruz.
        var convInfo = await _db.Conversations.AsNoTracking()
            .Where(x => x.Tenant.TenantKey == tenantKey && x.Bot.BotKey == botKey && x.SessionId == sessionId)
            .Select(x => new { x.Id, x.SessionId, x.CreatedAtUtc, x.LastActiveAtUtc })
            .FirstOrDefaultAsync(ct)
            ?? throw new KeyNotFoundException("İşletme, bot veya oturum bulunamadı.");

        var messages = await _db.Messages.AsNoTracking()
            .Where(m => m.ConversationId == convInfo.Id)
            .OrderByDescending(m => m.CreatedAtUtc)
            .Take(takeLast)
            .OrderBy(m => m.CreatedAtUtc) 
            .Select(m => new MessageDto(m.Id, m.Role, m.Text, m.CreatedAtUtc))
            .ToListAsync(ct);

        return new ConversationDto(convInfo.Id, convInfo.SessionId, convInfo.CreatedAtUtc, convInfo.LastActiveAtUtc, messages);
    }

    // ---- Yardımcı Metotlar ----

    private async Task<Guid> EnsureConversationAsync(string tenantKey, string botKey, string sessionId, CancellationToken ct)
    {
        // Botun aktifliğini ve Tenant aidiyetini tek sorguda kontrol et
        var botInfo = await _db.Bots.AsNoTracking()
            .Where(b => b.BotKey == botKey && b.Tenant.TenantKey == tenantKey && b.IsActive)
            .Select(b => new { b.Id, b.TenantId })
            .FirstOrDefaultAsync(ct)
            ?? throw new UnauthorizedAccessException("Geçersiz işletme veya bot.");

        var conv = await _db.Conversations
            .FirstOrDefaultAsync(x => x.BotId == botInfo.Id && x.SessionId == sessionId, ct);

        if (conv is null)
        {
            conv = new Conversation
            {
                TenantId = botInfo.TenantId,
                BotId = botInfo.Id,
                SessionId = sessionId,
                CreatedAtUtc = DateTime.UtcNow,
                LastActiveAtUtc = DateTime.UtcNow
            };
            _db.Conversations.Add(conv);
        }
        else
        {
            conv.LastActiveAtUtc = DateTime.UtcNow; // Aktiviteyi güncelle
        }

        await _db.SaveChangesAsync(ct);
        return conv.Id;
    }
}