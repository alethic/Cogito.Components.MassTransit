using System;

using MassTransit.Azure.ServiceBus.Core;

namespace Cogito.MassTransit.Azure.ServiceBus.Options
{

    /// <summary>
    /// Configuration for a specific receive endpoint.
    /// </summary>
    public class ServiceBusReceiveEndpointOptions
    {

        /// <summary>
        /// Number of concurrent calls to allow.
        /// </summary>
        public int? MaxConcurrentCalls { get; set; }

        /// <summary>
        /// Number of messages to prefetch.
        /// </summary>
        public int? PrefetchCount { get; set; }

        /// <summary>
        /// Duration to hold message locks.
        /// </summary>
        public TimeSpan? LockDuration { get; set; }

        /// <summary>
        /// Sets the TimeSpan structure that defines the default message TTL.
        /// </summary>
        public TimeSpan? DefaultMessageTimeToLive { get; set; }

        /// <summary>
        /// Sets the TimeSpan structure that defines the duration of the duplicate detection history. The default value is 10 minutes
        /// </summary>
        public TimeSpan? DuplicateDetectionHistoryTimeWindow { get; set; }

        /// <summary>
        /// If express is enabled, messages are not persisted to durable storage
        /// </summary>
        public bool? EnableExpress { get; set; }

        /// <summary>
        /// Sets a value that indicates whether the queue to be partitioned across multiple message brokers is enabled
        /// </summary>
        public bool? EnablePartitioning { get; set; }

        /// <summary>
        /// Sets a value that indicates whether the message is anonymous accessible.
        /// </summary>
        public bool? IsAnonymousAccessible { get; set; }

        /// <summary>
        /// Sets the maximum size of the queue in megabytes, which is the size of memory allocated for the queue
        /// </summary>
        public int? MaxSizeInMegabytes { get; set; }

        /// <summary>
        /// Sets the value indicating if this queue requires duplicate detection.
        /// </summary>
        public bool? RequiresDuplicateDetection { get; set; }

        /// <summary>
        /// Sets a value that indicates whether the queue supports ordering.
        /// </summary>
        public bool? SupportOrdering { get; set; }

        /// <summary>
        /// Message session timeout period.
        /// </summary>
        public TimeSpan? MessageWaitTimeout { get; set; }

        /// <summary>
        /// Duration at which to renew lock.
        /// </summary>
        public TimeSpan? LockRenewDelay { get; set; }

        /// <summary>
        /// Applies the options to the configurator.
        /// </summary>
        /// <param name="configurator"></param>
        public void Apply(IServiceBusReceiveEndpointConfigurator configurator)
        {
            if (MaxConcurrentCalls != null)
                configurator.MaxConcurrentCalls = (int)MaxConcurrentCalls;
            if (PrefetchCount != null)
                configurator.PrefetchCount = (int)PrefetchCount;
            if (LockDuration != null)
                configurator.LockDuration = (TimeSpan)LockDuration;
            if (DefaultMessageTimeToLive != null)
                configurator.DefaultMessageTimeToLive = (TimeSpan)DefaultMessageTimeToLive;
            if (DuplicateDetectionHistoryTimeWindow != null)
                configurator.DuplicateDetectionHistoryTimeWindow = (TimeSpan)DuplicateDetectionHistoryTimeWindow;
            if (EnablePartitioning != null)
                configurator.EnablePartitioning = (bool)EnablePartitioning;
            if (MaxSizeInMegabytes != null)
                configurator.MaxSizeInMegabytes = (int)MaxSizeInMegabytes;
            if (MessageWaitTimeout != null)
                configurator.MessageWaitTimeout = (TimeSpan)MessageWaitTimeout;
            if (RequiresDuplicateDetection != null)
                configurator.RequiresDuplicateDetection = (bool)RequiresDuplicateDetection;
        }

    }

}
