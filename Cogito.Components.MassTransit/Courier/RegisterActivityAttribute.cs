using System;

using Autofac.Builder;

using Cogito.Autofac;

namespace Cogito.Components.MassTransit.Courier
{

    /// <summary>
    /// Registers the type with the container to provide an activity host.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RegisterActivityAttribute :
        RegisterTypeAttribute,
        IRegistrationBuilderAttribute
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="executeEndpointName"></param>
        public RegisterActivityAttribute(string executeEndpointName)
        {
            ExecuteEndpointName = executeEndpointName ?? throw new ArgumentNullException(nameof(executeEndpointName));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="executeEndpointName"></param>
        /// <param name="compensateEndpointName"></param>
        public RegisterActivityAttribute(string executeEndpointName, string compensateEndpointName) :
            this(executeEndpointName)
        {
            CompensateEndpointName = compensateEndpointName ?? throw new ArgumentNullException(nameof(compensateEndpointName));
        }

        /// <summary>
        /// Name of the endpoint this activity will listen on.
        /// </summary>
        public string ExecuteEndpointName { get; set; }

        /// <summary>
        /// Name of the compensate endpoint of this activity type.
        /// </summary>
        public string CompensateEndpointName { get; set; }

        public IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> Build<TLimit, TActivatorData, TRegistrationStyle>(
            Type type,
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.WithMetadata<IActivityMetadata>(i => i
                .For(j => j.ExecuteEndpointName, ExecuteEndpointName)
                .For(j => j.CompensateEndpointName, CompensateEndpointName));
        }

    }

}
