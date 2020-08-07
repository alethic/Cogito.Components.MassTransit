using System;
using System.Collections.Generic;
using System.Linq;

using Cogito.Autofac;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Returns configuration for a given endpoint name.
    /// </summary>
    [RegisterAs(typeof(IBusFactoryDefinitionProvider))]
    [RegisterSingleInstance]
    public class RegistryBusFactoryConfigurationProvider :
        IBusFactoryDefinitionProvider
    {

        readonly IEnumerable<IBusFactoryDefinition> configuration;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="configuration"></param>
        public RegistryBusFactoryConfigurationProvider(
            IEnumerable<IBusFactoryDefinition> configuration)
        {
            this.configuration = configuration?.ToList() ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Finds all <see cref="IBusFactoryDefinition"/> registered with the container.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBusFactoryDefinition> GetDefinitions()
        {
            return configuration;
        }

    }

}
