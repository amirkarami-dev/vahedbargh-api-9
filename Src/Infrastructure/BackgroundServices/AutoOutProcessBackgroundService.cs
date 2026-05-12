using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Features.ElectProjectProcesses.Commands.AutoCancel;

namespace Coreapi.Infrastructure.BackgroundServices
{
    public class AutoOutProcessBackgroundService : IHostedService, IDisposable
    {
        private readonly IServiceProvider provider;
        private Timer _timer;

        public AutoOutProcessBackgroundService(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.Now;

            // Calculate the time until the next midnight
            //var nextMidnight = new DateTime(now.Year, now.Month, now.Day + 1, 0, 0, 0);
            //var timeUntilMidnight = nextMidnight - now;

            _timer = new Timer(Process, null, TimeSpan.FromMinutes(10),
                TimeSpan.FromMinutes(1440));

            return Task.CompletedTask;
        }

        private async void Process(object state)
        {
            using var scope = provider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(new AutoCancelCommand());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
