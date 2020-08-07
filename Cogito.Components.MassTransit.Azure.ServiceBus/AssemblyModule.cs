using Autofac;

using Cogito.Autofac;
using Cogito.Components.MassTransit;

using MassTransit;

namespace Cogito.Components.MassTransit.Azure.ServiceBus
{

    /// <summary>
    /// Provides <see cref="IBus"/> and <see cref="IBus"/> components across Azure Service Bus.
    /// </summary>
    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterModule<Cogito.Components.MassTransit.AssemblyModule>();
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
            builder.RegisterType<ServiceBusBusProvider>().As<IBusProvider>().AsSelf().SingleInstance();
        }

    }

}
