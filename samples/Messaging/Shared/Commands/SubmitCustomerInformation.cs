namespace Shared.Commands;

public class SubmitCustomerInformation : ICommand
{
    public Guid CustomerId { get; set; }
    public string Location { get; set; }
}