using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;

using Cogito.Collections;

using MassTransit;
using MassTransit.Courier;
using MassTransit.Internals.Extensions;

namespace Cogito.Components.MassTransit.Courier
{

    /// <summary>
    /// Provides various extension methods for loading configuration from the container.
    /// </summary>
    public static class AutofacActivityExtensions
    {

        /// <summary>
        /// Attaches <see cref="ExecuteActivity{TArguments}"/> to the given receive endpoint which have metadata
        /// specifying the given endpoint name.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="context"></param>
        /// <param name="endpointName"></param>
        public static void LoadExecuteActivities(
            this IReceiveEndpointConfigurator configurator,
            IComponentContext context,
            string endpointName)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));

            // load execute activities
            foreach (var (activityType, compensateEndpointName) in GetExecuteActivityTypesForEndpoint(context, endpointName).Distinct())
                ConfigureExecuteActivity(configurator, activityType, compensateEndpointName, context);
        }

        /// <summary>
        /// Configures an execute activity.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="activityType"></param>
        /// <param name="compensateEndpointName"></param>
        /// <param name="context"></param>
        static void ConfigureExecuteActivity(IReceiveEndpointConfigurator configurator, Type activityType, string compensateEndpointName, IComponentContext context)
        {
            ExecuteActivityConfiguratorCache.Configure(
                activityType,
                activityType.GetClosingArguments(typeof(IExecuteActivity<>)).First(),
                configurator,
                context.Resolve<ILifetimeScope>(),
                BuildCompensateAddress(configurator.InputAddress, compensateEndpointName));
        }

        /// <summary>
        /// Gets the registered compensate activities for the given endpoint.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        static IEnumerable<(Type Type, string CompensateEndpointName)> GetExecuteActivityTypesForEndpoint(IComponentContext context, string endpointName)
        {
            // execute activities with matching endpoint name
            return context.ComponentRegistry.Registrations
                .SelectMany(r => r.Services.OfType<IServiceWithType>(), (r, s) => new { r, s })
                .Where(rs => rs.s.ServiceType.HasInterface<IExecuteActivity>())
                .Where(rs => (string)rs.r.Metadata.GetOrDefault("ExecuteEndpointName") == endpointName)
                .Select(rs => (rs.s.ServiceType, (string)rs.r.Metadata.GetOrDefault("CompensateEndpointName")))
                .Distinct();
        }

        /// <summary>
        /// Builds a new endpoint address for the given base address and endpoint name.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        static Uri BuildCompensateAddress(Uri baseUri, string endpointName)
        {
            return endpointName != null ? new Uri(baseUri, "/" + endpointName) : null;
        }

        /// <summary>
        /// Attaches <see cref="CompensateActivity{TLog}"/> to the given receive endpoint which have metadata
        /// specifying the given endpoint name.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="context"></param>
        /// <param name="endpointName"></param>
        /// <param name="name"></param>
        public static void LoadCompensateActivities(
            this IReceiveEndpointConfigurator configurator,
            IComponentContext context,
            string endpointName)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));

            // load compensate activities
            foreach (var type in GetCompensateTypesForEndpoint(context, endpointName).Distinct())
                ConfigureCompensateActivity(configurator, type, context);
        }

        /// <summary>
        /// Configures the a compensate activity.
        /// </summary>
        /// <param name="configurator"></param>
        /// <param name="activityType"></param>
        /// <param name="context"></param>
        static void ConfigureCompensateActivity(IReceiveEndpointConfigurator configurator, Type activityType, IComponentContext context)
        {
            CompensateActivityConfiguratorCache.Configure(
                activityType,
                activityType.GetClosingArguments(typeof(ICompensateActivity<>)).First(),
                configurator,
                context.Resolve<ILifetimeScope>());
        }

        /// <summary>
        /// Gets the registered compensate activity types for the given endpoint name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="endpointName"></param>
        /// <returns></returns>
        static IEnumerable<Type> GetCompensateTypesForEndpoint(IComponentContext context, string endpointName)
        {
            return context.ComponentRegistry.Registrations
                .SelectMany(r => r.Services.OfType<IServiceWithType>(), (r, s) => new { r, s })
                .Where(rs => rs.s.ServiceType.HasInterface<ICompensateActivity>())
                .Where(rs => (string)rs.r.Metadata.GetOrDefault("CompensateEndpointName") == endpointName)
                .Select(rs => rs.s.ServiceType)
                .Distinct();
        }

    }

}
