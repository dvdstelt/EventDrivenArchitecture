using Shared.Events;

namespace Sender.Handlers;

public class CustomerInformationModifiedHandler : IHandleMessages<CustomerInformationModified>
{
    public Task Handle(CustomerInformationModified message, IMessageHandlerContext context)
    {
        Console.WriteLine($"So customer {message.CustomerId} was modified");
        
        return Task.CompletedTask;
    }
}