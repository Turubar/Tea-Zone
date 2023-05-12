namespace TeaZone.Models.Entities;

public partial class RedTeaTable : ITeaProduct
{
    public long Id { get; set; }

    public long IdProduct { get; set; }

    public long IdForm { get; set; }

    public string? ProductSet { get; set; }

    public string? Price { get; set; }

    public string? Description { get; set; }

    public virtual Form IdFormNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
