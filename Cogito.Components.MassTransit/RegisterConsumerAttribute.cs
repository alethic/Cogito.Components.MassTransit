using System;

using Autofac.Builder;

using Cogito.Autofac;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Registers the class as a <see cref="IConsumer"/> to consume messages on a given receive endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterConsumerAttribute :
        RegisterTypeAttribute,
        IRegistrationBuilderAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="endpointName"></param>
        public RegisterConsumerAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="endpointName"></param>
        public RegisterConsumerAttribute(string endpointName)
        {
            EndpointName = endpointName ?? throw new ArgumentNullException(nameof(endpointName));
        }

        /// <summary>
        /// Name of the endpoint this type applies to.
        /// </summary>
        public string EndpointName { get; set; }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> Build<TLimit, TActivatorData, TRegistrationStyle>(
            Type type,
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.WithMetadata<IReceiveEndpointMetadata>(i => i.For(j => j.EndpointName, EndpointName));
        }

    }

}
