namespace Shared.NServiceBus;

public static class EndpointConfigurationExtensions
{
    public static EndpointConfiguration ApplyDefaultConfiguration(this EndpointConfiguration endpointConfiguration,
        Action<RoutingSettings<LearningTransport>> configureRouting = null!)
    {
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.Recoverability().Delayed(c => c.NumberOfRetries(0));

        var transport = endpointConfiguration.UseTransport<LearningTransport>();
        var routing = transport.Routing();
        configureRouting?.Invoke(routing);

        endpointConfiguration.UsePersistence<LearningPersistence>();

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningCommandsAs(t =>
            t.Namespace != null && t.Namespace.StartsWith("Shared") && t.Namespace.EndsWith("Commands"));
        conventions.DefiningEventsAs(t =>
            t.Namespace != null && t.Namespace.StartsWith("Shared") && t.Namespace.EndsWith("Events"));

        return endpointConfiguration;
    }
}