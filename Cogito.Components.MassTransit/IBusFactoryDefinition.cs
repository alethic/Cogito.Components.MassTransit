using MassTransit;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Describes a class which can apply configuration to a bus.
    /// </summary>
    public interface IBusFactoryDefinition
    {

        /// <summary>
        /// Configures the factory.
        /// </summary>
        /// <param name="configurator"></param>
        void Apply(IBusFactoryConfigurator configurator);

    }

}