using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;

using Cogito.Autofac;
using Cogito.Collections;
using Cogito.Components.MassTransit;

using MassTransit.Courier;
using MassTransit.Internals.Extensions;

namespace Cogito.Components.MassTransit.Courier
{

    /// <summary>
    /// Provides receive endpoint names from registered compensate activity metadata.
    /// </summary>
    [RegisterAs(typeof(IReceiveEndpointNameProvider))]
    public class CompensateActivityEndpointNameProvider :
        IReceiveEndpointNameProvider
    {

        readonly IComponentContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public CompensateActivityEndpointNameProvider(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<string> GetEndpointNames()
        {
            // registrations of type ICompensateActivity need EndpointName metadata
            return context.ComponentRegistry.Registrations
                .SelectMany(r => r.Services.OfType<IServiceWithType>(), (r, s) => new { r, s })
                .Where(rs => rs.s.ServiceType.HasInterface(typeof(ICompensateActivity)))
                .Select(rs => (string)rs.r.Metadata.GetOrDefault("CompensateEndpointName"))
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .Distinct();
        }

    }

}
