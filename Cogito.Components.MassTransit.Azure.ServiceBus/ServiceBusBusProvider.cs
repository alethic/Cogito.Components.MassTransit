using System;
using System.Linq;

using Cogito.Components.MassTransit;
using Cogito.MassTransit.Azure.ServiceBus.Options;

using MassTransit;
using MassTransit.Azure.ServiceBus.Core;

using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cogito.Components.MassTransit.Azure.ServiceBus
{

    /// <summary>
    /// Provides the MassTransit bus instance.
    /// </summary>
    public class ServiceBusBusProvider : IBusProvider, IDisposable
    {

        readonly IOptions<ServiceBusOptions> options;
        readonly IOrderedEnumerable<IBusFactoryDefinitionProvider> busDefinitionProviders;
        readonly IOrderedEnumerable<IReceiveEndpointNameProvider> endpointNameProviders;
        readonly IOrderedEnumerable<IReceiveEndpointDefinitionProvider> endpointDefinitionProviders;
        readonly ILogger logger;

        readonly ServiceBusConnectionStringBuilder busConnectionString;
        readonly Lazy<Uri> busAddress;
        readonly Lazy<IBusControl> bus;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="busDefinitionProviders"></param>
        /// <param name="endpointNameProviders"></param>
        /// <param name="endpointDefinitionProviders"></param>
        /// <param name="logger"></param>
        public ServiceBusBusProvider(
            IOptions<ServiceBusOptions> options,
            IOrderedEnumerable<IBusFactoryDefinitionProvider> busDefinitionProviders,
            IOrderedEnumerable<IReceiveEndpointNameProvider> endpointNameProviders,
            IOrderedEnumerable<IReceiveEndpointDefinitionProvider> endpointDefinitionProviders,
            ILogger logger)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.busDefinitionProviders = busDefinitionProviders ?? throw new ArgumentNullException(nameof(busDefinitionProviders));
            this.endpointNameProviders = endpointNameProviders ?? throw new ArgumentNullException(nameof(endpointNameProviders));
            this.endpointDefinitionProviders = endpointDefinitionProviders ?? throw new ArgumentNullException(nameof(endpointDefinitionProviders));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            busConnectionString = BuildConnectionString();
            busAddress = new Lazy<Uri>(CreateBusAddress, true);
            bus = new Lazy<IBusControl>(CreateBus, true);
        }

        /// <summary>
        /// Gets the address of the bus.
        /// </summary>
        public Uri BusAddress => busAddress.Value;

        /// <summary>
        /// Gets the control interface of the provided bus.
        /// </summary>
        public IBusControl Bus => bus.Value;

        /// <summary>
        /// Builds the connection string.
        /// </summary>
        /// <returns></returns>
        ServiceBusConnectionStringBuilder BuildConnectionString()
        {
            if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
                throw new ConfigurationException("Invalid or missing ServiceBus connection string.");

            // parse and check connection string
            var connectionString = new ServiceBusConnectionStringBuilder(options.Value.ConnectionString);
            if (connectionString.Endpoint == null)
                throw new ConfigurationException("No ServiceBus endpoint specified.");

            return connectionString;
        }

        /// <summary>
        /// Creates the bus address.
        /// </summary>
        /// <returns></returns>
        Uri CreateBusAddress()
        {
            return new Uri(busConnectionString.Endpoint);
        }

        /// <summary>
        /// Creates the Mass Transit Bus instance.
        /// </summary>
        /// <returns></returns> 
        IBusControl CreateBus()
        {
            return global::MassTransit.Bus.Factory.CreateUsingAzureServiceBus(factoryCfg =>
            {
                // configure service bus host
                factoryCfg.Host(busConnectionString.ToString(), hostCfg => { });

                factoryCfg.UseServiceBusMessageScheduler();
                factoryCfg.AutoDeleteOnIdle = TimeSpan.FromHours(1);

                // configure any additional configuration
                foreach (var configuration in busDefinitionProviders.SelectMany(i => i.GetDefinitions()).Distinct())
                    if (configuration != null)
                        configuration.Apply(factoryCfg);

                // register any provided endpoints
                foreach (var endpointName in endpointNameProviders.SelectMany(i => i.GetEndpointNames()).Distinct())
                    if (endpointName != null)
                        factoryCfg.ReceiveEndpoint(endpointName, endpointCfg => ConfigureReceiveEndpoint(endpointName, endpointCfg));
            });
        }

        /// <summary>
        /// Applies the available configuration registered for the endpoint to the endpoint.
        /// </summary>
        /// <param name="endpointName"></param>
        /// <param name="endpoint"></param>
        void ConfigureReceiveEndpoint(string endpointName, IServiceBusReceiveEndpointConfigurator endpoint)
        {
            if (endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));

            // apply extra configuration
            var definitions = endpointDefinitionProviders
                .SelectMany(i => i.GetDefinitions(endpointName))
                .Distinct()
                .ToList();

            foreach (var definition in definitions)
                if (definition != null)
                    definition.Apply(endpoint);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            if (bus.IsValueCreated)
                bus.Value.Stop();
        }

    }

}
