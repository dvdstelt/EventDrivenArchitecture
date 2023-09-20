var config = new EndpointConfiguration("Receiver");
config.UseTransport(new LearningTransport());

var endpoint = await Endpoint.Start(config);

Console.WriteLine("Waiting for messages...");
Console.ReadKey();

await endpoint.Stop();