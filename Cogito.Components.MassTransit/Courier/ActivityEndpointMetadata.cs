namespace Cogito.Components.MassTransit.Courier
{

    public class ActivityEndpointMetadata :
        ReceiveEndpointMetadata
    {

        /// <summary>
        /// Name of the compensation endpoint.
        /// </summary>
        public string CompensateEndpointName { get; set; }

    }

}

