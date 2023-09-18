using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Events;

namespace Sales.Handlers;

public class AcceptOrderHandler : IHandleMessages<AcceptOrder>
{
    private readonly ILogger<AcceptOrderHandler> log;

    public AcceptOrderHandler(ILogger<AcceptOrderHandler> log)
    {
        this.log = log;
    }
    
    public async Task Handle(AcceptOrder message, IMessageHandlerContext context)
    {
        log.LogInformation("Received AcceptOrder for OrderId [{OrderId}]", message.OrderId);

        await context.Publish(new OrderAccepted()
        {
            OrderId = message.OrderId
        });
    }
}