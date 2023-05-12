namespace TeaZone.Models.Entities;

public partial class Form
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<DarkTeaTable> DarkTeaTables { get; } = new List<DarkTeaTable>();

    public virtual ICollection<GreenTeaTable> GreenTeaTables { get; } = new List<GreenTeaTable>();

    public virtual ICollection<OolongTable> OolongTables { get; } = new List<OolongTable>();

    public virtual ICollection<PuerTable> PuerTables { get; } = new List<PuerTable>();

    public virtual ICollection<RedTeaTable> RedTeaTables { get; } = new List<RedTeaTable>();

    public virtual ICollection<WhiteTeaTable> WhiteTeaTables { get; } = new List<WhiteTeaTable>();
}
