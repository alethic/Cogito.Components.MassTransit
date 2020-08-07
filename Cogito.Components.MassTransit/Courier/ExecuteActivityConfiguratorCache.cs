using System;
using System.Collections.Concurrent;

using Autofac;

using MassTransit;
using MassTransit.Courier;

namespace Cogito.Components.MassTransit.Courier
{

    /// <summary>
    /// Caches configurator instances for the given execute activity types.
    /// </summary>
    static class ExecuteActivityConfiguratorCache
    {

        /// <summary>
        /// Gets the <see cref="ExecuteActivityConfigurator"/> for the given <see cref="Type"/>.
        /// </summary>
        /// <param name="activityType"></param>
        /// <returns></returns>
        static ExecuteActivityConfigurator GetOrAdd(Type activityType, Type argumentType)
        {
            return Cached.Instance.GetOrAdd(activityType, _ => (ExecuteActivityConfigurator)Activator.CreateInstance(typeof(ExecuteActivityConfigurator<,>).MakeGenericType(activityType, argumentType)));
        }

        public static void Configure(Type activityType, Type argumentType, IReceiveEndpointConfigurator configurator, ILifetimeScope scope, Uri compensateAddress)
        {
            GetOrAdd(activityType, argumentType).Configure(configurator, scope, compensateAddress);
        }

        static class Cached
        {

            internal static readonly ConcurrentDictionary<Type, ExecuteActivityConfigurator> Instance = new ConcurrentDictionary<Type, ExecuteActivityConfigurator>();

        }

        interface ExecuteActivityConfigurator
        {

            void Configure(IReceiveEndpointConfigurator configurator, ILifetimeScope scope, Uri compensateAddress);

        }

        class ExecuteActivityConfigurator<TActivity, TArguments> :
            ExecuteActivityConfigurator
            where TActivity : class, IExecuteActivity<TArguments>
            where TArguments : class
        {

            public void Configure(IReceiveEndpointConfigurator configurator, ILifetimeScope scope, Uri compensateAddress)
            {
                if (compensateAddress != null)
                    configurator.ExecuteActivityHost<TActivity, TArguments>(compensateAddress, scope);
                else
                    configurator.ExecuteActivityHost<TActivity, TArguments>(scope);
            }

        }

    }

}
