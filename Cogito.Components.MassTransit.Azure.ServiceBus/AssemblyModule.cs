using Autofac;

using Cogito.Autofac;

namespace Cogito.Components.MassTransit.Azure.ServiceBus
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterModule<Cogito.MassTransit.Azure.ServiceBus.Autofac.AssemblyModule>();
            builder.RegisterModule<Cogito.Components.MassTransit.AssemblyModule>();
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
