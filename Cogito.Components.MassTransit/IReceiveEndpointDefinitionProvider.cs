using System.Collections.Generic;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Provides configuration for a given receive endpoint.
    /// </summary>
    public interface IReceiveEndpointDefinitionProvider
    {

        /// <summary>
        /// Gets the configurations to apply to the endpoint of the given name.
        /// </summary>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        IEnumerable<IReceiveEndpointDefinition> GetDefinitions(string endpointName);

    }

}