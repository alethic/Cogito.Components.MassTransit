using MassTransit;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Describes a class which can apply configuration to a receive endpoint.
    /// </summary>
    public interface IReceiveEndpointDefinition
    {

        /// <summary>
        /// Configures the endpoint.
        /// </summary>
        /// <param name="configurator"></param>
        void Apply(IReceiveEndpointConfigurator configurator);

    }

}