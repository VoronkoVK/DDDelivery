namespace DeliveryApp.Core;

public class Settings
{
    public string ConnectionString { get; set; }
    public string GeoServiceGrpcHost { get; set; }
    public string MessageBrokerHost { get; set; }
    public string OrderEventsTopic { get; set; }
    public string BasketEventsTopic { get; set; }
}