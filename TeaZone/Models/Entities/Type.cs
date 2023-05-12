namespace TeaZone.Models.Entities;

public partial class Type
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string RelatedTable { get; set; } = null!;

    public byte[]? Image1 { get; set; }

    public byte[]? Image2 { get; set; }

    public byte[]? Image3 { get; set; }

    public byte[]? Image4 { get; set; }

    public byte[]? Image5 { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
