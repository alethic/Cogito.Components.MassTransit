using System;

using MassTransit;
using MassTransit.Registration;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Provides a Mass Transit bus.
    /// </summary>
    public interface IBusProvider
    {

        /// <summary>
        /// Gets the address of the bus.
        /// </summary>
        Uri BusAddress { get; }

        /// <summary>
        /// Creates the bus.
        /// </summary>
        /// <returns></returns>
        IBusControl Bus { get; }

    }

}
