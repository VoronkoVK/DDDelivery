using Testcontainers.PostgreSql;
using Xunit;

namespace CntFixtures;

public sealed class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; } =
        new PostgreSqlBuilder(image: "postgres:14.7")
            .WithDatabase("basket")
            .WithUsername("username")
            .WithPassword("secret")
            .Build();

    public string ConnectionString => Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}