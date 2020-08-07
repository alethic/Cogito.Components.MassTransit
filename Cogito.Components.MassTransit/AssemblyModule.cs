using Autofac;

using Cogito.Autofac;

using MassTransit;
using MassTransit.AutofacIntegration;
using MassTransit.AutofacIntegration.Registration;
using MassTransit.AutofacIntegration.ScopeProviders;
using MassTransit.Context;
using MassTransit.Monitoring.Health;
using MassTransit.Registration;
using MassTransit.Scoping;
using MassTransit.Transactions;
using MassTransit.Transports;

namespace Cogito.Components.MassTransit
{

    public class AssemblyModule : ModuleBase
    {

        IConsumerScopeProvider CreateConsumerScopeProvider(IComponentContext context)
        {
            var lifetimeScopeProvider = new SingleLifetimeScopeProvider(context.Resolve<ILifetimeScope>());
            return new AutofacConsumerScopeProvider(lifetimeScopeProvider, "message", (b, c) => { });
        }

        static ISendEndpointProvider GetCurrentSendEndpointProvider(IComponentContext context)
        {
            if (context.TryResolve(out ConsumeContext consumeContext))
                return consumeContext;

            var bus = context.ResolveOptional<ITransactionalBus>() ?? context.Resolve<IBus>();
            return new ScopedSendEndpointProvider<ILifetimeScope>(bus, context.Resolve<ILifetimeScope>());
        }

        static IPublishEndpoint GetCurrentPublishEndpoint(IComponentContext context)
        {
            if (context.TryResolve(out ConsumeContext consumeContext))
                return consumeContext;

            var bus = context.ResolveOptional<ITransactionalBus>() ?? context.Resolve<IBus>();
            return new PublishEndpoint(new ScopedPublishEndpointProvider<ILifetimeScope>(bus, context.Resolve<ILifetimeScope>()));
        }

        protected override void Register(ContainerBuilder builder)
        {
            builder.RegisterFromAttributes(typeof(AssemblyModule).Assembly);

            builder.RegisterType<BusDepot>()
                .As<IBusDepot>()
                .SingleInstance();

            builder.Register(context => new BusHealth(nameof(IBus)))
                .As<BusHealth>()
                .As<IBusHealth>()
                .SingleInstance();

            builder.Register(GetCurrentSendEndpointProvider)
                .As<ISendEndpointProvider>()
                .InstancePerLifetimeScope();

            builder.Register(GetCurrentPublishEndpoint)
                .As<IPublishEndpoint>()
                .InstancePerLifetimeScope();

            //builder.Register(context => ClientFactoryProvider(context.Resolve<IConfigurationServiceProvider>(), context.Resolve<IBus>()))
            //    .As<IClientFactory>()
            //    .SingleInstance();

            builder.Register(CreateConsumerScopeProvider)
                .As<IConsumerScopeProvider>()
                .SingleInstance()
                .IfNotRegistered(typeof(IConsumerScopeProvider));

            builder.Register(context => new AutofacConfigurationServiceProvider(context.Resolve<ILifetimeScope>()))
                .As<IConfigurationServiceProvider>()
                .SingleInstance()
                .IfNotRegistered(typeof(IConfigurationServiceProvider));

            //builder.Register(CreateRegistrationContext)
            //    .As<IBusRegistrationContext>()
            //    .SingleInstance();

            // register service bus
            builder.Register(ctx => ctx.Resolve<IBusProvider>().Bus).As<IBus>().As<IBusControl>().SingleInstance().ExternallyOwned();

            // setup global logging
            builder.RegisterBuildCallback(i => LogContext.ConfigureCurrentLogContext(i.Resolve<Microsoft.Extensions.Logging.ILoggerFactory>()));
        }

    }

}
