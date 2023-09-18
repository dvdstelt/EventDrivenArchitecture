namespace Shared.Commands;

public class AcceptOrder
{
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
}