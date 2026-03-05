using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatbotSaaS.Application.DTOs.Chat;

namespace ChatbotSaaS.Application.Abstractions
{
    public interface ILlmService
    {
        Task<LlmChatResult> GenerateAsync(
        string systemPrompt,
        string model,
        double temperature,
        int maxOutputTokens,
        IReadOnlyList<LlmMessage> messages,
        CancellationToken ct);
    }
}
