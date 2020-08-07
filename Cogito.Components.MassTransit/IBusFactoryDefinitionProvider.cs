using System.Collections.Generic;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Provides configuration for the default MassTransit Bus.
    /// </summary>
    public interface IBusFactoryDefinitionProvider
    {

        /// <summary>
        /// Gets the configurations to apply to the bus factory.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IBusFactoryDefinition> GetDefinitions();

    }

}