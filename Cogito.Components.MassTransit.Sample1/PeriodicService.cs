using System;
using System.Threading;
using System.Threading.Tasks;

using Cogito.Autofac;

using MassTransit;

using Microsoft.Extensions.Hosting;

namespace Cogito.Components.MassTransit.Sample1
{

    [RegisterAs(typeof(IHostedService))]
    class PeriodicService : IHostedService
    {

        readonly IBus bus;
        readonly System.Timers.Timer timer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public PeriodicService(IBus bus)
        {
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));

            timer = new System.Timers.Timer();
        }

        void OnTimer(object sender, EventArgs args)
        {
            bus.Publish<TestMessage>(new TestMessage()).GetAwaiter().GetResult();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer.Interval = 5000;
            timer.Elapsed += OnTimer;
            timer.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Stop();
            timer.Elapsed -= OnTimer;
            return Task.CompletedTask;
        }

    }
}
