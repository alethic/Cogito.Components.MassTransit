using System;
using System.Collections.Generic;
using System.Linq;

using Cogito.Autofac;

using MassTransit;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Configures the registered <see cref="IBusObserver"/>s.
    /// </summary>
    [RegisterAs(typeof(IBusFactoryDefinition))]
    [RegisterSingleInstance]
    [RegisterOrder(-100)]
    public class BusFactoryObserverDefinition : IBusFactoryDefinition
    {

        readonly IEnumerable<IBusObserver> observers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="observers"></param>
        public BusFactoryObserverDefinition(IEnumerable<IBusObserver> observers)
        {
            this.observers = observers?.ToList() ?? throw new ArgumentNullException(nameof(observers));
        }

        /// <summary>
        /// Applies the configuration to the bus.
        /// </summary>
        /// <param name="configurator"></param>
        public void Apply(IBusFactoryConfigurator configurator)
        {
            foreach (var observer in observers)
                if (observer != null)
                    configurator.ConnectBusObserver(observer);
        }

    }

}
