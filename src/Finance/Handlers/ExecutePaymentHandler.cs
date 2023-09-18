using Finance.Commands;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace Finance.Handlers;

public class ExecutePaymentHandler : IHandleMessages<ExecutePayment>
{
    private readonly ILogger<ExecutePaymentHandler> log;

    public ExecutePaymentHandler(ILogger<ExecutePaymentHandler> log)
    {
        this.log = log;
    }

    public async Task Handle(ExecutePayment message, IMessageHandlerContext context)
    {
        log.LogInformation("Received ExecutePayment for OrderId [{OrderId}]", message.OrderId);

        // Talk to third party payment provider
        
        await context.Publish(new PaymentSucceeded() { OrderId = message.OrderId });
    }
}