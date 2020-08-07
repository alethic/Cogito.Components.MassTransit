using System;

using Autofac;

using Cogito.Components.MassTransit.Courier;

using MassTransit;

namespace Cogito.Components.MassTransit.Courier
{

    /// <summary>
    /// Loads execute activities from Autofac onto the receive endpoint.
    /// </summary>
    [RegisterReceiveEndpointDefinition]
    public class AutofacExecuteActivityReceiveEndpointConfiguration :
        IReceiveEndpointDefinition
    {

        readonly ILifetimeScope scope;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="scope"></param>
        public AutofacExecuteActivityReceiveEndpointConfiguration(ILifetimeScope scope)
        {
            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
        }

        public void Apply(IReceiveEndpointConfigurator configurator)
        {
            configurator.LoadExecuteActivities(scope, configurator.InputAddress.LocalPath.Trim('/'));
        }

    }

}
