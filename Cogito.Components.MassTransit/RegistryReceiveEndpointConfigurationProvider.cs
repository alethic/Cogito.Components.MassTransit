using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using Cogito.Autofac;

using MassTransit;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Returns configuration for a given endpoint name.
    /// </summary>
    [RegisterAs(typeof(IReceiveEndpointDefinitionProvider))]
    public class RegistryReceiveEndpointConfigurationProvider :
        IReceiveEndpointDefinitionProvider
    {

        readonly IEnumerable<(IReceiveEndpointDefinition Value, ReceiveEndpointMetadata Metadata)> config;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="config"></param>
        public RegistryReceiveEndpointConfigurationProvider(IOrderedEnumerable<Lazy<IReceiveEndpointDefinition, ReceiveEndpointMetadata>> config)
        {
            this.config = config?.Select(i => (i.Value, i.Metadata))?.ToList() ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Finds all <see cref="IConsumer"/> types with the appropriate metadata for the given receive endpoint name.
        /// </summary>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        public IEnumerable<IReceiveEndpointDefinition> GetDefinitions(string endpointName)
        {
            return config
                .Where(i => i.Metadata?.EndpointName == null || i.Metadata.EndpointName == endpointName)
                .Select(i => i.Value)
                .Where(i => i != null)
                .Distinct();
        }

    }

}
