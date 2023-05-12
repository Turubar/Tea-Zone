namespace TeaZone.Models.Entities;

public partial class Product
{
    public long Id { get; set; }

    public long IdType { get; set; }

    public string Name { get; set; } = null!;

    public byte[]? Image1 { get; set; }

    public byte[]? Image2 { get; set; }

    public byte[]? Image3 { get; set; }

    public byte[]? Image4 { get; set; }

    public byte[]? Image5 { get; set; }

    public virtual ICollection<DarkTeaTable> DarkTeaTables { get; } = new List<DarkTeaTable>();

    public virtual ICollection<GreenTeaTable> GreenTeaTables { get; } = new List<GreenTeaTable>();

    public virtual Type IdTypeNavigation { get; set; } = null!;

    public virtual ICollection<OolongTable> OolongTables { get; } = new List<OolongTable>();

    public virtual ICollection<PuerTable> PuerTables { get; } = new List<PuerTable>();

    public virtual ICollection<RedTeaTable> RedTeaTables { get; } = new List<RedTeaTable>();

    public virtual ICollection<WhiteTeaTable> WhiteTeaTables { get; } = new List<WhiteTeaTable>();
}
