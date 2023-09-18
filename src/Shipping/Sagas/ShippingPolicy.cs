using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Events;

namespace Finance.Sagas;

public class ShippingPolicyData : ContainSagaData
{
    public Guid OrderId { get; set; }
    public Guid Address { get; set; }
    public string ShippingMethod { get; set; }
    public bool PaymentSucceeded { get; set; } = false;
}

public class ShippingPolicy : Saga<ShippingPolicyData>,
    IHandleMessages<SubmitShippingAddress>,
    IHandleMessages<SubmitShippingMethod>,
    IHandleMessages<PaymentSucceeded>
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
            .ToMessage<PaymentSucceeded>(m => m.OrderId);
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

    private async Task VerifyIfOkayToContinue(IMessageHandlerContext context)
    {
        if (Data.PaymentSucceeded && !string.IsNullOrEmpty(Data.ShippingMethod) &&
            !string.IsNullOrEmpty(Data.ShippingMethod))
        {
            log.LogInformation("Cool, we\'re done for OrderId [{OrderId}]", Data.OrderId);
            await context.Publish(new OrderShipped() { OrderId = Data.OrderId });
        }
    }
}