﻿using System.Threading.Tasks;

using Autofac.Extensions.DependencyInjection;

using Cogito.Autofac;
using Cogito.Extensions.Options;
using Cogito.Extensions.Options.Autofac;
using Cogito.MassTransit.Azure.ServiceBus.Options;

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
                    .Configure<ServiceBusOptions>(o => o.ConnectionString = "")
                    .RegisterAllAssemblyModules()))
                .ConfigureAppConfiguration(b => b.AddEnvironmentVariables())
                .Build()
                .RunAsync();
        }

    }

}
