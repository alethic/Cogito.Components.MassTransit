using Autofac;

using Cogito.Autofac;

namespace Cogito.Components.MassTransit
{

    public class AssemblyModule : ModuleBase
    {

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterModule<Cogito.MassTransit.Autofac.AssemblyModule>();
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);
        }

    }

}
