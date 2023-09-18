namespace Shared.Commands;

public class SubmitShippingAddress
{
    public Guid OrderId { get; set; }
    public Guid Address { get; set; }
}