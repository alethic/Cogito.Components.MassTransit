using System.Threading.Tasks;

using Autofac.Extensions.DependencyInjection;

using Cogito.Autofac;
using Cogito.MassTransit.Autofac;
using Cogito.MassTransit.Azure.ServiceBus.Autofac;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Cogito.Components.MassTransit.Sample2
{

    public static class Program
    {

        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(b => b
                    .RegisterMassTransitBus(i => i
                        .UsingAzureServiceBus(c => c.ConnectionString = "Endpoint=sb://revelwe-dev1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=E6k1J6j9v+8wKOA/kJyHvHv6wx65J3B4+FXIZ9kXNug=")
                        .WithHostedService())
                    .RegisterAllAssemblyModules()))
                .ConfigureAppConfiguration(b => b.AddEnvironmentVariables())
                .Build()
                .RunAsync();
        }

    }

}
