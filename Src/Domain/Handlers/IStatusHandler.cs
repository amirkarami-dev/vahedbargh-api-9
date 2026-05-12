namespace Coreapi.Domain.Handlers
{
    public interface IStatusHandler
    {
        bool HasErrors { get; }
        void AddError(string message, string field);
        string GetError();
    }
}
