using System;

using Autofac;

using Cogito.Components.MassTransit;

using MassTransit;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Loads consumers from Autofac onto the receive endpoint.
    /// </summary>
    [RegisterReceiveEndpointDefinition]
    public class AutofacConsumerReceiveEndpointDefinition : IReceiveEndpointDefinition
    {

        readonly ILifetimeScope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="scope"></param>
        public AutofacConsumerReceiveEndpointDefinition(ILifetimeScope scope)
        {
            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public void Apply(IReceiveEndpointConfigurator configurator)
        {
            configurator.LoadConsumers(scope, configurator.InputAddress.LocalPath.Trim('/'));
        }

    }

}
