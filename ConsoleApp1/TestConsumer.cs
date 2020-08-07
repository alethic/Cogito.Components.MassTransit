using System.Threading.Tasks;

using Cogito.Components.MassTransit;

using MassTransit;

using Microsoft.Extensions.Logging;

namespace ConsoleApp1
{

    [RegisterConsumer("test_endpoint")]
    public class TestConsumer : IConsumer<TestMessage>
    {

        readonly ILogger logger;

        public TestConsumer(ILogger logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<TestMessage> context)
        {
            logger.LogInformation("Received");
            return Task.CompletedTask;
        }

    }

}
