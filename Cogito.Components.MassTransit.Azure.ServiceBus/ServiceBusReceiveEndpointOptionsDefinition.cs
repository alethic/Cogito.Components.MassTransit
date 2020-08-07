using System;

using Cogito.Collections;
using Cogito.Components.MassTransit;
using Cogito.MassTransit.Azure.ServiceBus.Options;

using MassTransit;
using MassTransit.Azure.ServiceBus.Core;

using Microsoft.Extensions.Options;

namespace Cogito.Components.MassTransit.Azure.ServiceBus
{

    /// <summary>
    /// Applies user specified endpoint configuration.
    /// </summary>
    [RegisterReceiveEndpointDefinition]
    public class ServiceBusReceiveEndpointOptionsDefinition : IReceiveEndpointDefinition
    {

        readonly IOptions<ServiceBusOptions> options;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public ServiceBusReceiveEndpointOptionsDefinition(IOptions<ServiceBusOptions> options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Apply(IReceiveEndpointConfigurator configurator)
        {
            if (configurator is IServiceBusReceiveEndpointConfigurator sb)
            {
                var ep = options.Value.Endpoints?.GetOrDefault(configurator.InputAddress.LocalPath);
                if (ep != null)
                    ep.Apply(sb);
            }
        }

    }

}
