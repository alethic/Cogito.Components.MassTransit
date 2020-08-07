namespace Cogito.Components.MassTransit
{

    /// <summary>
    /// Metadata attached to a class which indicates which receive endpoint it will be applicable to.
    /// </summary>
    public interface IReceiveEndpointMetadata
    {

        /// <summary>
        /// Name of the receive endpoint to which the class is applicable.
        /// </summary>
        string EndpointName { get; }

    }

}
