using System.Collections.Concurrent;

namespace Coreapi.Infrastructure.BaleBot;

public enum BaleConversationStage
{
    SelectingLoginType,        // /start → show کاربر / مالک buttons
    WaitingForUsername,        // After clicking کاربر
    WaitingForPassword,
    WaitingForLandlordNaCode,  // After clicking مالک
    Authenticated,             // Login succeeded; role menu visible
    WaitingForFileNumber,      // Expecting file-number text (all auth roles)
    WaitingForElectRequestNum, // Expecting ElectRequestNumber text (admin roles)
}

public class BaleConversationState
{
    public BaleConversationStage Stage { get; set; } = BaleConversationStage.SelectingLoginType;
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
