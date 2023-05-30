namespace WebApplicationOrdersMicroService.Requests;

public sealed record CreateOrUpdateDishRequest
{
    public string SenderName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
