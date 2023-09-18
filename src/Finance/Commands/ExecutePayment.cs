namespace Finance.Commands;

public class ExecutePayment
{
    public Guid OrderId { get; set; }
    public Guid PaymentMethod { get; set; }
}