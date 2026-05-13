using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Coreapi.Infrastructure.BaleBot.Models;

public class BaleUpdate
{
    [JsonPropertyName("update_id")]
    public long UpdateId { get; set; }

    [JsonPropertyName("message")]
    public BaleMessage? Message { get; set; }

    [JsonPropertyName("callback_query")]
    public BaleCallbackQuery? CallbackQuery { get; set; }
}

public class BaleMessage
{
    [JsonPropertyName("message_id")]
    public long MessageId { get; set; }

    [JsonPropertyName("from")]
    public BaleUser? From { get; set; }

    [JsonPropertyName("chat")]
    public BaleChat? Chat { get; set; }

    [JsonPropertyName("date")]
    public long Date { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class BaleCallbackQuery
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("from")]
    public BaleUser? From { get; set; }

    /// <summary>Message the button was attached to.</summary>
    [JsonPropertyName("message")]
    public BaleMessage? Message { get; set; }

    /// <summary>Data from the pressed InlineKeyboardButton.</summary>
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}

public class BaleUser
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("is_bot")]
    public bool IsBot { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }
}

public class BaleChat
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }
}

public class BaleApiResponse<T>
{
    [JsonPropertyName("ok")]
    public bool Ok { get; set; }

    [JsonPropertyName("result")]
    public T? Result { get; set; }

    [JsonPropertyName("error_code")]
    public int? ErrorCode { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

public class BaleSendMessageRequest
{
    [JsonPropertyName("chat_id")]
    public long ChatId { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("reply_markup")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public InlineKeyboardMarkup? ReplyMarkup { get; set; }
}

public class InlineKeyboardMarkup
{
    [JsonPropertyName("inline_keyboard")]
    public List<List<InlineKeyboardButton>> InlineKeyboard { get; set; } = new();
}

public class InlineKeyboardButton
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("callback_data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CallbackData { get; set; }
}
