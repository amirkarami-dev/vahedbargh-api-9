using System.Threading;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Interfaces;

public interface IBaleService
{
    Task ProcessUpdateAsync(long chatId, string text, CancellationToken cancellationToken = default);
    Task HandleCallbackQueryAsync(long chatId, string callbackQueryId, string data, CancellationToken cancellationToken = default);
    Task SendMessageAsync(long chatId, string text, CancellationToken cancellationToken = default);
}
