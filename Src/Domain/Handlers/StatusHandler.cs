namespace Coreapi.Domain.Handlers
{
    public class StatusHandler : IStatusHandler
    {
        public string Message { get; private set; }
        public string Field { get; private set; }

        public bool HasErrors => !(string.IsNullOrEmpty(Message) && string.IsNullOrEmpty(Field));

        public void AddError(string message, string field)
        {
            Message = message;
            Field = field;
        }

        public string GetError()
        {
            return $"{Message} ({Field})";
        }
    }
}
