namespace TeaZone.Models.Entities;

public partial class Account
{
    public long Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string EmailVerify { get; set; } = null!;

    public byte[]? Avatar { get; set; }

    public long IdRole { get; set; }

    public string? Cart { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
