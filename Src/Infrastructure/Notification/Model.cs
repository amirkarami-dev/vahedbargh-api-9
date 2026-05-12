namespace Coreapi.Infrastructure.Notification;


public class UserConnection
{
    public string UserId { get; set; }
    public string ConnectionId { get; set; }
}

public class UserToken
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public Platform Platform { get; set; } // Enum for iOS, Android, etc.
}

public enum Platform
{
    Mobile = 0,
    Web = 1
}