using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Infrastructure.BaleBot.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Coreapi.Infrastructure.BaleBot;

public class BalePollingService(
    IServiceScopeFactory scopeFactory,
    IHttpClientFactory httpClientFactory,
    ILogger<BalePollingService> logger) : BackgroundService
{
    private long _offset;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Bale bot polling service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PollOnceAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Bale polling error. Retrying in 5 seconds.");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        logger.LogInformation("Bale bot polling service stopped.");
    }

    private async Task PollOnceAsync(CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("BaleBot");
        var url = $"getUpdates?offset={_offset}&timeout=30";

        var response = await client.GetFromJsonAsync<BaleApiResponse<List<BaleUpdate>>>(url, cancellationToken);

        if (response is not { Ok: true } || response.Result is not { Count: > 0 })
            return;

        foreach (var update in response.Result)
        {
            _offset = update.UpdateId + 1;

            await using var scope = scopeFactory.CreateAsyncScope();
            var baleService = scope.ServiceProvider.GetRequiredService<IBaleService>();

            try
            {
                // Inline button press
                if (update.CallbackQuery is { } cb && cb.Message?.Chat != null)
                {
                    await baleService.HandleCallbackQueryAsync(
                        cb.Message.Chat.Id,
                        cb.Id,
                        cb.Data ?? string.Empty,
                        cancellationToken);
                    continue;
                }

                // Regular text message
                if (update.Message is { Chat: not null } msg)
                {
                    await baleService.ProcessUpdateAsync(
                        msg.Chat.Id,
                        msg.Text ?? string.Empty,
                        cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing Bale update {UpdateId}", update.UpdateId);
            }
        }
    }
}
