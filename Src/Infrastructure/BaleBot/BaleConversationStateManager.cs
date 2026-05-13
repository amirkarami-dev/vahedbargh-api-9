using System.Collections.Concurrent;

namespace Coreapi.Infrastructure.BaleBot;

public enum BaleConversationStage
{
    WaitingForUsername,
    WaitingForPassword,
    Authenticated
}

public class BaleConversationState
{
    public BaleConversationStage Stage { get; set; } = BaleConversationStage.WaitingForUsername;
    public string? PendingUsername { get; set; }
}

public class BaleConversationStateManager
{
    private readonly ConcurrentDictionary<long, BaleConversationState> _states = new();

    public BaleConversationState GetOrCreate(long chatId) =>
        _states.GetOrAdd(chatId, _ => new BaleConversationState());

    public void Reset(long chatId) =>
        _states[chatId] = new BaleConversationState();
}
