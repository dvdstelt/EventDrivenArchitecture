namespace Shared.Events;

public class CustomerInformationModified : IEvent
{
    public Guid CustomerId { get; set; }
}