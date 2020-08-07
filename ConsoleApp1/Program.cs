using System.Threading.Tasks;

using Autofac.Extensions.DependencyInjection;

using Cogito.Autofac;
using Cogito.Extensions.Options;
using Cogito.Extensions.Options.Autofac;
using Cogito.MassTransit.Azure.ServiceBus.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ConsoleApp1
{

    public static class Program
    {

        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(b => b
                    .Configure<ServiceBusOptions>(o => o.ConnectionString = "Endpoint=sb://revelwe-dev1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=MFPVqnUyMzSXHmx9G7eNEXOlsJ0aAjDqND0cnvqLikU=")
                    .RegisterAllAssemblyModules()))
                .ConfigureAppConfiguration(b => b.AddEnvironmentVariables())
                .Build()
                .RunAsync();
        }

    }

}
