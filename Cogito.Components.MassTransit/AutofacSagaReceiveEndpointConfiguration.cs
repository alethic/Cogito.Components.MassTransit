//using System;

//using Autofac;

//using MassTransit;

//namespace Cogito.MassTransit
//{

//    /// <summary>
//    /// Loads consumers and sagas from Autofac onto the receive endpoint.
//    /// </summary>
//    [RegisterReceiveEndpointConfiguration]
//    public class AutofacSagaReceiveEndpointConfiguration :
//        IReceiveEndpointConfiguration
//    {

//        readonly ILifetimeScope scope;

//        /// <summary>
//        /// Initializes a new instance.
//        /// </summary>
//        /// <param name="scope"></param>
//        public AutofacSagaReceiveEndpointConfiguration(ILifetimeScope scope)
//        {
//            this.scope = scope ?? throw new ArgumentNullException(nameof(scope));
//        }

//        public void Apply(IReceiveEndpointConfigurator configurator)
//        {
//            configurator.LoadSagas(scope, configurator.InputAddress.LocalPath.Trim('/'));
//        }

//    }

//}
