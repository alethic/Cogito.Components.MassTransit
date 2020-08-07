using Cogito.Autofac;
using Cogito.Components.MassTransit;
using Cogito.MassTransit.Azure.ServiceBus.Options;

using MassTransit;
using MassTransit.Azure.ServiceBus.Core;

using Microsoft.Extensions.Options;

namespace Cogito.Components.MassTransit.Azure.ServiceBus
{

    /// <summary>
    /// Applies registered options to the bus.
    /// </summary>
    [RegisterAs(typeof(IBusFactoryDefinition))]
    [RegisterSingleInstance]
    public class ServiceBusBusFactoryOptionsDefinition : IBusFactoryDefinition
    {

        readonly IOptions<ServiceBusOptions> options;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        public ServiceBusBusFactoryOptionsDefinition(IOptions<ServiceBusOptions> options = null)
        {
            this.options = options;
        }

        /// <summary>
        /// Applies the configuration to the bus.
        /// </summary>
        /// <param name="configurator"></param>
        public void Apply(IBusFactoryConfigurator configurator)
        {
            if (options != null)
                if (configurator is IServiceBusBusFactoryConfigurator sb)
                    options.Value.Apply(sb);
        }

    }

}
