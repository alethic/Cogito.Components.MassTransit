using System;
using System.Collections.Concurrent;

using Autofac;

using MassTransit;
using MassTransit.Courier;

namespace Cogito.Components.MassTransit.Courier
{

    /// <summary>
    /// Caches configurator instances for the given compensate activity types.
    /// </summary>
    static class CompensateActivityConfiguratorCache
    {

        /// <summary>
        /// Gets the <see cref="CompensateActivityConfigurator"/> for the given <see cref="Type"/>.
        /// </summary>
        /// <param name="activityType"></param>
        /// <returns></returns>
        static CompensateActivityConfigurator GetOrAdd(Type activityType, Type logType)
        {
            return Cached.Instance.GetOrAdd(activityType, _ => (CompensateActivityConfigurator)Activator.CreateInstance(typeof(CompensateActivityConfigurator<,>).MakeGenericType(activityType, logType)));
        }

        public static void Configure(Type activityType, Type logType, IReceiveEndpointConfigurator configurator, ILifetimeScope scope)
        {
            GetOrAdd(activityType, logType).Configure(configurator, scope);
        }

        static class Cached
        {

            internal static readonly ConcurrentDictionary<Type, CompensateActivityConfigurator> Instance = new ConcurrentDictionary<Type, CompensateActivityConfigurator>();

        }

        interface CompensateActivityConfigurator
        {

            void Configure(IReceiveEndpointConfigurator configurator, ILifetimeScope scope);

        }

        class CompensateActivityConfigurator<TActivity, TLog> :
            CompensateActivityConfigurator
            where TActivity : class, ICompensateActivity<TLog>
            where TLog : class
        {

            public void Configure(IReceiveEndpointConfigurator configurator, ILifetimeScope scope)
            {
                configurator.CompensateActivityHost<TActivity, TLog>(scope);
            }

        }

    }

}
