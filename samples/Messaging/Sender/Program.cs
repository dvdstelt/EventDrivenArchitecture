using Shared.Commands;

var config = new EndpointConfiguration("Sender");
var routing = config.UseTransport(new LearningTransport());
routing.RouteToEndpoint(typeof(SubmitCustomerInformation), "Receiver");

var endpoint = await Endpoint.Start(config);


Console.WriteLine("Press a key to stop this endpoint");

while (true)
{
    var result = Console.ReadLine();
    
    var msg = new SubmitCustomerInformation
    {
        CustomerId = Guid.NewGuid(),
        Location = result
    };

    await endpoint.Send(msg);
    Console.WriteLine("Send message");
}

await endpoint.Stop();