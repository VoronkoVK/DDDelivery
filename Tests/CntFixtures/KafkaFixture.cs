using Testcontainers.Kafka;
using Xunit;

namespace CntFixtures;

public sealed class KafkaFixture : IAsyncLifetime
{
    public KafkaContainer Container { get; } =
        new KafkaBuilder(image: "confluentinc/cp-kafka:6.2.1")
            .Build();
    public string BootstrapServers => Container.GetBootstrapAddress();

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}