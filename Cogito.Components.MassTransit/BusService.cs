using System;
using System.Threading;
using System.Threading.Tasks;

using Cogito.Autofac;

using MassTransit;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Ensures the bus is started at application launch.
    /// </summary>
    [RegisterAs(typeof(IHostedService))]
    public class BusService : IHostedService
    {

        readonly IBusControl bus;
        readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="logger"></param>
        public BusService(IBusControl bus, ILogger logger)
        {
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await bus.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                await bus.StopAsync(cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception occurred shutting down Bus.");
            }
        }

    }

}
