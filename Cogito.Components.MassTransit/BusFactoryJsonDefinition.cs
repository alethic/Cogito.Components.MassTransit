using System;
using System.Collections.Generic;
using System.Diagnostics;

using Cogito.Autofac;
using Cogito.Collections;

using MassTransit;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Configures the JSON features of the bus.
    /// </summary>
    [RegisterAs(typeof(IBusFactoryDefinition))]
    [RegisterSingleInstance]
    public class BusFactoryJsonDefinition : IBusFactoryDefinition
    {

        readonly IEnumerable<JsonConverter> converters;
        readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="converters"></param>
        /// <param name="logger"></param>
        public BusFactoryJsonDefinition(IEnumerable<JsonConverter> converters, ILogger logger)
        {
            this.converters = converters ?? throw new ArgumentNullException(nameof(converters));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Configures the factory.
        /// </summary>
        /// <param name="configurator"></param>
        public void Apply(IBusFactoryConfigurator configurator)
        {
            configurator.ConfigureJsonSerializer(Configure);
            configurator.ConfigureJsonDeserializer(Configure);
        }

        /// <summary>
        /// Configures the settings.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        JsonSerializerSettings Configure(JsonSerializerSettings settings)
        {
            settings.TraceWriter = new LoggingTraceWriter(logger);
            settings.Converters.AddRange(converters);
            settings.Formatting = Formatting.None;
            settings.DateParseHandling = DateParseHandling.None;
            return settings;
        }

        /// <summary>
        /// Provides a <see cref="ITraceWriter"/> that logs to the internal logging infrastructure.
        /// </summary>
        class LoggingTraceWriter : ITraceWriter
        {

            readonly ILogger logger;

            public LoggingTraceWriter(ILogger logger)
            {
                this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            /// <summary>
            /// Gets or sets the logging level.
            /// </summary>
            public TraceLevel LevelFilter { get; set; }

            public void Trace(TraceLevel level, string message, Exception exception)
            {
                // ignore levels being filtered
                if (level < LevelFilter)
                    return;

                if (exception != null)
                    TraceException(level, message, exception);
                else
                    Trace(level, message);
            }

            void Trace(TraceLevel level, string message)
            {
                switch (level)
                {
                    case TraceLevel.Off:
                        break;
                    case TraceLevel.Error:
                        logger.LogError(message);
                        break;
                    case TraceLevel.Warning:
                        logger.LogWarning(message);
                        break;
                    case TraceLevel.Info:
                        logger.LogInformation(message);
                        break;
                    case TraceLevel.Verbose:
                        logger.LogDebug(message);
                        break;
                    default:
                        break;
                }
            }

            void TraceException(TraceLevel level, string message, Exception exception)
            {
                switch (level)
                {
                    case TraceLevel.Off:
                        break;
                    case TraceLevel.Error:
                        logger.LogError(exception, message);
                        break;
                    case TraceLevel.Warning:
                        logger.LogWarning(exception, message);
                        break;
                    case TraceLevel.Info:
                        logger.LogInformation(exception, message);
                        break;
                    case TraceLevel.Verbose:
                        logger.LogDebug(exception, message);
                        break;
                    default:
                        break;
                }
            }

        }

    }

}
