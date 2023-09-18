namespace Shared.Commands;

public class SubmitPayment
{
    public Guid OrderId { get; set; }
    public Guid PaymentMethod { get; set; }
}