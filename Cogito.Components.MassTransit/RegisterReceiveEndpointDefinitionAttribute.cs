using System;

using Autofac.Builder;

using Cogito.Autofac;

namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Registers the class as a <see cref="IReceiveEndpointDefinition"/> to apply configuration during creation of
    /// a MassTransit receive endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterReceiveEndpointDefinitionAttribute : RegisterTypeAttribute, IRegistrationBuilderAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public RegisterReceiveEndpointDefinitionAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="endpointName"></param>
        public RegisterReceiveEndpointDefinitionAttribute(string endpointName)
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
            return builder
                .As<IReceiveEndpointDefinition>()
                .WithMetadata<ReceiveEndpointMetadata>(i => i.For(j => j.EndpointName, EndpointName));
        }

    }

}
