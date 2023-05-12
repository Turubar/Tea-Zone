using TeaZone.Models.Entities;

public interface ITeaProduct
{
    public long Id { get; set; }

    public long IdProduct { get; set; }

    public long IdForm { get; set; }

    public string? ProductSet { get; set; }

    public string? Price { get; set; }

    public string? Description { get; set; }
}