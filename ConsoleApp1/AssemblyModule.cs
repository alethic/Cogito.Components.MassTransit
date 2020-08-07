using Autofac;

using Cogito.Autofac;
using Cogito.Components.MassTransit;

using MassTransit;

namespace ConsoleApp1
{

    /// <summary>
    /// Provides <see cref="IBus"/> and <see cref="IBus"/> components across Azure Service Bus.
    /// </summary>
    public class AssembyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssembyModule).Assembly);
        }

    }

}
