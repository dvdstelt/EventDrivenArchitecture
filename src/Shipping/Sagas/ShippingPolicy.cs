using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Events;

namespace Shipping.Sagas;

public class ShippingPolicy : Saga<ShippingPolicyData>,
    IAmStartedByMessages<SubmitShippingAddress>,
    IAmStartedByMessages<SubmitShippingMethod>,
    IAmStartedByMessages<PaymentSucceeded>,
    IAmStartedByMessages<OrderAccepted>
{
    private readonly ILogger<ShippingPolicy> log;

    public ShippingPolicy(ILogger<ShippingPolicy> log)
    {
        this.log = log;
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
    {
        mapper.MapSaga(saga => saga.OrderId)
            .ToMessage<SubmitShippingAddress>(m => m.OrderId)
            .ToMessage<SubmitShippingMethod>(m => m.OrderId)
            .ToMessage<PaymentSucceeded>(m => m.OrderId)
            .ToMessage<OrderAccepted>(m => m.OrderId);
    }

    public async Task Handle(SubmitShippingAddress message, IMessageHandlerContext context)
    {
        log.LogInformation("Received [SubmitShippingAddress] for OrderId [{OrderId}]", message.OrderId);
        Data.Address = message.Address;
        await VerifyIfOkayToContinue(context);
    }

    public async Task Handle(SubmitShippingMethod message, IMessageHandlerContext context)
    {
        log.LogInformation("Received [SubmitShippingMethod] for OrderId [{OrderId}]", message.OrderId);
        Data.ShippingMethod = message.ShippingMethod;
        await VerifyIfOkayToContinue(context);
    }

    public async Task Handle(PaymentSucceeded message, IMessageHandlerContext context)
    {
        log.LogInformation("Received [PaymentSucceeded] for OrderId [{OrderId}]", message.OrderId);
        Data.PaymentSucceeded = true;
        await VerifyIfOkayToContinue(context);
    }

    public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
    {
        log.LogInformation("Received [OrderAccepted] for OrderId [{OrderId}]", message.OrderId);

        Data.OrderAccepted = true;
        await VerifyIfOkayToContinue(context);
    }

    private async Task VerifyIfOkayToContinue(IMessageHandlerContext context)
    {
        if (Data.PaymentSucceeded &&
            Data.OrderAccepted &&
            !string.IsNullOrEmpty(Data.ShippingMethod) &&
            Data.Address != Guid.Empty)
        {
            log.LogInformation("Cool, we\'ve received all for OrderId [{OrderId}]", Data.OrderId);
            await context.Publish(new OrderShipped() { OrderId = Data.OrderId });
        }
    }
}