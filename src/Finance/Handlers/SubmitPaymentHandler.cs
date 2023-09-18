using Finance.Commands;
using Microsoft.Extensions.Logging;
using Shared.Commands;

namespace Finance.Handlers;

public class SubmitPaymentHandler : IHandleMessages<SubmitPayment>
{
    private readonly ILogger<SubmitPaymentHandler> log;

    public SubmitPaymentHandler(ILogger<SubmitPaymentHandler> log)
    {
        this.log = log;
    }

    public async Task Handle(SubmitPayment message, IMessageHandlerContext context)
    {
        log.LogInformation("Received SubmitPayment for OrderId [{OrderId}]", message.OrderId);

        // Store payment information

        // Execute the payment
        await context.Send(new ExecutePayment() { OrderId = message.OrderId, PaymentMethod = message.PaymentMethod });
    }
}