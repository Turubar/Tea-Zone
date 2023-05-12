namespace TeaZone.Models.Entities;

public partial class Order
{
    public long Id { get; set; }

    public long OrderNumber { get; set; }

    public string Date { get; set; } = null!;

    public string Products { get; set; } = null!;

    public string Price { get; set; } = null!;

    public string? NameClient { get; set; }

    public string? PhoneNumberClient { get; set; }
}
