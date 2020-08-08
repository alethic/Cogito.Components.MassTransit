using Autofac;

using Cogito.Autofac;

using MassTransit;

namespace Cogito.Components.MassTransit.Sample2
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
