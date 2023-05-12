namespace TeaZone.Models.Entities;

public partial class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();
}
