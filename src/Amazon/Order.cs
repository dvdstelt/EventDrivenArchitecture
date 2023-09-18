namespace Amazon;

public class Order
{
    public Guid OrderId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal Price { get; } = 42;
    public Guid ProductId { get; } = Guid.Parse("79620f95-ed84-40cd-ac45-344965ecdd8d");
    public string ShippingMethod { get; set; }
    public string PaymentMethod { get; set; }
    public string ShippingAddress { get; set; }
}