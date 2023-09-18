namespace Shared.Commands;

public class SubmitShippingMethod
{
    public Guid OrderId { get; set; }
    public string ShippingMethod { get; set; }
}