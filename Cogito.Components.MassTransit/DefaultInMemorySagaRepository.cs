using Cogito.Autofac;

using MassTransit.Saga;

using Microsoft.Extensions.Logging;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Default in-memory repository. Replace this repository instance with a concrete persistent instance in
    /// production.
    /// </summary>
    /// <typeparam name="TSaga"></typeparam>
    [RegisterAs(typeof(ISagaRepository<>))]
    [RegisterSingleInstance]
    public class DefaultInMemorySagaRepository<TSaga> : InMemorySagaRepository<TSaga>
        where TSaga : class, ISaga
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DefaultInMemorySagaRepository(ILogger logger) :
            base()
        {
            logger?.LogWarning("Using default InMemorySagaRepository. This repository is not persistent. Change to a durable repository before releasing to production.");
        }

    }

}
