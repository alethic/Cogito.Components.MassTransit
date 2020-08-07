using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;

using Cogito.Collections;

using MassTransit;
using MassTransit.Internals.Extensions;
using MassTransit.Saga;
using MassTransit.Scoping;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Provides various extension methods for loading configuration from the container.
    /// </summary>
    public static class AutofacExtensions
    {

        /// <summary>
        /// Attaches Consumers to the specified receive endpoint which have metadata referencing the
        /// given endpoint name.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="context"></param>
        /// <param name="endpointName"></param>
        /// <param name="name"></param>
        public static void LoadConsumers(
            this IReceiveEndpointConfigurator configurator,
            IComponentContext context,
            string endpointName,
            string name = "message")
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            // load consumers
            foreach (var t in GetConsumerTypesForEndpoint(context, endpointName).Distinct())
                ConsumerConfiguratorCache.Configure(t, configurator, context.Resolve<IConsumerScopeProvider>());
        }

        /// <summary>
        /// Gets the registered consumer types for the given endpoint name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        static IEnumerable<Type> GetConsumerTypesForEndpoint(IComponentContext context, string endpointName)
        {
            return context.ComponentRegistry.Registrations
                .SelectMany(r => r.Services.OfType<IServiceWithType>(), (r, s) => new { r, s })
                .Where(rs => rs.s.ServiceType.HasInterface<IConsumer>())
                .Where(rs => (string)rs.r.Metadata.GetOrDefault("EndpointName") == endpointName)
                .Select(rs => rs.s.ServiceType)
                .Where(t => !t.HasInterface<ISaga>())
                .Distinct();
        }

        ///// <summary>
        ///// Attaches Sagas to the specified receive endpoint which have metadata referencing the given endpoint name.
        ///// </summary>
        ///// <param name="configurator"></param>
        ///// <param name="context"></param>
        ///// <param name="endpointName"></param>
        ///// <param name="name"></param>
        //public static void LoadSagas(
        //    this IReceiveEndpointConfigurator configurator,
        //    IComponentContext context,
        //    string endpointName,
        //    string name = "message")
        //{
        //    if (context == null)
        //        throw new ArgumentNullException(nameof(context));
        //    if (configurator == null)
        //        throw new ArgumentNullException(nameof(configurator));
        //    if (name == null)
        //        throw new ArgumentNullException(nameof(name));

        //    // load sagas
        //    var sl = new SingleLifetimeScopeProvider(context.Resolve<ILifetimeScope>());
        //    var rf = new AutofacSagaRepositoryFactory(sl, name, (builder, consumeContext) => { });
        //    foreach (var t in GetSagaTypesForEndpoint(context, endpointName).Distinct())

        //}

        ///// <summary>
        ///// Gets the registered saga types for the given endpoint name.
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="endpointName"></param>
        ///// <returns></returns>
        //static IEnumerable<Type> GetSagaTypesForEndpoint(IComponentContext context, string endpointName)
        //{
        //    return context.ComponentRegistry.Registrations
        //        .SelectMany(r => r.Services.OfType<IServiceWithType>(), (r, s) => new { r, s })
        //        .Where(rs => rs.s.ServiceType.HasInterface<ISaga>())
        //        .Where(rs => (string)rs.r.Metadata.GetOrDefault("EndpointName") == endpointName)
        //        .Select(rs => rs.s.ServiceType)
        //        .Distinct();
        //}

    }

}
