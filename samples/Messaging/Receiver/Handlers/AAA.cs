using Shared.Events;

namespace Receiver.Handlers;

public class AAA : IHandleMessages<CustomerInformationModified>
{
    public Task Handle(CustomerInformationModified message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Received event for customer {message.CustomerId}");
        
        return Task.CompletedTask;
    }
}