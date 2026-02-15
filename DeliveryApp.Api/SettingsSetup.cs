using DeliveryApp.Core;
using Microsoft.Extensions.Options;

namespace DeliveryApp.Api;

public class SettingsSetup : IConfigureOptions<Settings>
{
    private readonly IConfiguration _configuration;

    public SettingsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(Settings options)
    {
        options.ConnectionString = _configuration["CONNECTION_STRING"];
        options.GeoServiceGrpcHost = _configuration["GEO_SERVICE_GRPC_HOST"];
        options.MessageBrokerHost = _configuration["MESSAGE_BROKER_HOST"];
        options.OrderEventsTopic = _configuration["ORDER_EVENTS_TOPIC"];
        options.BasketEventsTopic = _configuration["BASKET_EVENTS_TOPIC"];
    }
}