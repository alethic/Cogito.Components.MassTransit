using System.Collections.Generic;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Provides a set of receive endpoint names to be registered on the bus.
    /// </summary>
    public interface IReceiveEndpointNameProvider
    {

        /// <summary>
        /// Gets a set of endpoint names to register.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetEndpointNames();

    }

}
