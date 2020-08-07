using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;

using Cogito.Autofac;
using Cogito.Collections;

using MassTransit;
using MassTransit.Internals.Extensions;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Provides receive endpoint names from registered consumer metadata.
    /// </summary>
    [RegisterAs(typeof(IReceiveEndpointNameProvider))]
    [RegisterSingleInstance]
    public class RegistryConsumerEndpointNameProvider :
        IReceiveEndpointNameProvider
    {

        readonly IComponentContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public RegistryConsumerEndpointNameProvider(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<string> GetEndpointNames()
        {
            // registrations of type IConsumer need EndpointName metadata
            return context.ComponentRegistry.Registrations
                .SelectMany(r => r.Services.OfType<IServiceWithType>(), (r, s) => new { r, s })
                .Where(rs => rs.s.ServiceType.HasInterface<IConsumer>())
                .Select(rs => (string)rs.r.Metadata.GetOrDefault("EndpointName"))
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Distinct();
        }

    }

}
