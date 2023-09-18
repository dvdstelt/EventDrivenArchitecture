namespace Shipping.Sagas;

public class ShippingPolicyData : ContainSagaData
{
    public Guid OrderId { get; set; }
    public Guid Address { get; set; }
    public string ShippingMethod { get; set; }
    public bool PaymentSucceeded { get; set; } = false;
    public bool OrderAccepted { get; set; } = false;
}