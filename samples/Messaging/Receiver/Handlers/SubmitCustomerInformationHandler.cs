using Shared.Commands;
using Shared.Events;

namespace Receiver.Handlers;

public class SubmitCustomerInformationHandler : IHandleMessages<SubmitCustomerInformation>
{
    public async Task Handle(SubmitCustomerInformation message, IMessageHandlerContext context)
    {
        Console.WriteLine($"We received info for a customer in {message.Location}");

        var @event = new CustomerInformationModified
        {
            CustomerId = message.CustomerId
        };

        await context.Publish(@event);
        Console.WriteLine("Published event");
    }
}