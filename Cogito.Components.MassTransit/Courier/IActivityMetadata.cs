namespace Cogito.Components.MassTransit.Courier
{

    /// <summary>
    /// Describes the metadata for a component registered as an execute host.
    /// </summary>
    public interface IActivityMetadata
    {

        /// <summary>
        /// Name of the endpoing to register as the execute address of the activity.
        /// </summary>
        string ExecuteEndpointName { get; }

        /// <summary>
        /// Name of the endpoint to register as the compensate address of the execute activity.
        /// </summary>
        string CompensateEndpointName { get; }

    }

}
