using Microsoft.EntityFrameworkCore;

namespace TeaZone.Models.Entities;

public partial class TeaZoneDbContext : DbContext
{
    public TeaZoneDbContext()
    {
    }

    public TeaZoneDbContext(DbContextOptions<TeaZoneDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<DarkTeaTable> DarkTeaTables { get; set; }

    public virtual DbSet<Form> Forms { get; set; }

    public virtual DbSet<GreenTeaTable> GreenTeaTables { get; set; }

    public virtual DbSet<OolongTable> OolongTables { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PuerTable> PuerTables { get; set; }

    public virtual DbSet<RedTeaTable> RedTeaTables { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<WhiteTeaTable> WhiteTeaTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)=> optionsBuilder.UseSqlite("DataSource=wwwroot\\database\\TeaZoneDB.sqlite");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasIndex(e => e.Email, "Accounts_email_unique").IsUnique();

            entity.HasIndex(e => e.Login, "Accounts_login_unique").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.EmailVerify).HasColumnName("Email_verify");
            entity.Property(e => e.IdRole).HasColumnName("ID_role");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Accounts).HasForeignKey(d => d.IdRole);
        });

        modelBuilder.Entity<DarkTeaTable>(entity =>
        {
            entity.ToTable("DarkTeaTable");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdForm).HasColumnName("ID_form");
            entity.Property(e => e.IdProduct).HasColumnName("ID_product");
            entity.Property(e => e.ProductSet).HasColumnName("Product_set");

            entity.HasOne(d => d.IdFormNavigation).WithMany(p => p.DarkTeaTables).HasForeignKey(d => d.IdForm);

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.DarkTeaTables).HasForeignKey(d => d.IdProduct);
        });

        modelBuilder.Entity<Form>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<GreenTeaTable>(entity =>
        {
            entity.ToTable("GreenTeaTable");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdForm).HasColumnName("ID_form");
            entity.Property(e => e.IdProduct).HasColumnName("ID_product");
            entity.Property(e => e.ProductSet).HasColumnName("Product_set");

            entity.HasOne(d => d.IdFormNavigation).WithMany(p => p.GreenTeaTables).HasForeignKey(d => d.IdForm);

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.GreenTeaTables).HasForeignKey(d => d.IdProduct);
        });

        modelBuilder.Entity<OolongTable>(entity =>
        {
            entity.ToTable("OolongTable");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdForm).HasColumnName("ID_form");
            entity.Property(e => e.IdProduct).HasColumnName("ID_product");
            entity.Property(e => e.ProductSet).HasColumnName("Product_set");

            entity.HasOne(d => d.IdFormNavigation).WithMany(p => p.OolongTables).HasForeignKey(d => d.IdForm);

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OolongTables).HasForeignKey(d => d.IdProduct);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NameClient).HasColumnName("Name_client");
            entity.Property(e => e.OrderNumber).HasColumnName("Order_number");
            entity.Property(e => e.PhoneNumberClient).HasColumnName("Phone_number_client");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdType).HasColumnName("ID_type");
            entity.Property(e => e.Image1).HasColumnName("Image_1");
            entity.Property(e => e.Image2).HasColumnName("Image_2");
            entity.Property(e => e.Image3).HasColumnName("Image_3");
            entity.Property(e => e.Image4).HasColumnName("Image_4");
            entity.Property(e => e.Image5).HasColumnName("Image_5");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Products).HasForeignKey(d => d.IdType);
        });

        modelBuilder.Entity<PuerTable>(entity =>
        {
            entity.ToTable("PuerTable");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdForm).HasColumnName("ID_form");
            entity.Property(e => e.IdProduct).HasColumnName("ID_product");
            entity.Property(e => e.ProductSet).HasColumnName("Product_set");

            entity.HasOne(d => d.IdFormNavigation).WithMany(p => p.PuerTables).HasForeignKey(d => d.IdForm);

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.PuerTables).HasForeignKey(d => d.IdProduct);
        });

        modelBuilder.Entity<RedTeaTable>(entity =>
        {
            entity.ToTable("RedTeaTable");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdForm).HasColumnName("ID_form");
            entity.Property(e => e.IdProduct).HasColumnName("ID_product");
            entity.Property(e => e.ProductSet).HasColumnName("Product_set");

            entity.HasOne(d => d.IdFormNavigation).WithMany(p => p.RedTeaTables).HasForeignKey(d => d.IdForm);

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.RedTeaTables).HasForeignKey(d => d.IdProduct);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Image1).HasColumnName("Image_1");
            entity.Property(e => e.Image2).HasColumnName("Image_2");
            entity.Property(e => e.Image3).HasColumnName("Image_3");
            entity.Property(e => e.Image4).HasColumnName("Image_4");
            entity.Property(e => e.Image5).HasColumnName("Image_5");
            entity.Property(e => e.RelatedTable).HasColumnName("Related_table");
        });

        modelBuilder.Entity<WhiteTeaTable>(entity =>
        {
            entity.ToTable("WhiteTeaTable");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdForm).HasColumnName("ID_form");
            entity.Property(e => e.IdProduct).HasColumnName("ID_product");
            entity.Property(e => e.ProductSet).HasColumnName("Product_set");

            entity.HasOne(d => d.IdFormNavigation).WithMany(p => p.WhiteTeaTables).HasForeignKey(d => d.IdForm);

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.WhiteTeaTables).HasForeignKey(d => d.IdProduct);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
