using Ecommerce.Models;
using Ecommerce.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<TipoDocumento> TiposDocumento => Set<TipoDocumento>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<DiscProduct> DiscProducts => Set<DiscProduct>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");

            entity.Property(user => user.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(user => user.MiddleName)
                .HasMaxLength(100);

            entity.Property(user => user.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(user => user.SecondLastName)
                .HasMaxLength(100);

            entity.Property(user => user.TipoDoc)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasOne(user => user.TipoDocumento)
                .WithMany()
                .HasForeignKey(user => user.TipoDocumentoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(user => user.NumDocumento)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(user => user.CreatedAtUtc)
                .IsRequired();

            entity.HasIndex(user => user.CreatedAtUtc);
        });

        builder.Entity<TipoDocumento>(entity =>
        {
            entity.ToTable("TiposDocumento");

            entity.Property(tipoDocumento => tipoDocumento.Descripcion)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(tipoDocumento => tipoDocumento.Descripcion)
                .IsUnique();

            entity.HasData(
                new TipoDocumento { Id = 1, Descripcion = "Cedula", CountValid = 10 },
                new TipoDocumento { Id = 2, Descripcion = "Ruc", CountValid = 13 },
                new TipoDocumento { Id = 3, Descripcion = "Pasaporte", CountValid = null });
        });

        builder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.Property(product => product.Sku)
                .HasColumnName("sku")
                .HasMaxLength(64)
                .IsRequired();

            entity.HasIndex(product => product.Sku)
                .IsUnique();

            entity.Property(product => product.LongDescripcion)
                .HasColumnName("long_descripcion")
                .HasMaxLength(2000)
                .IsRequired();

            entity.Property(product => product.ShortDescripcion)
                .HasColumnName("short_descripcion")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(product => product.Stock)
                .HasColumnName("stock")
                .IsRequired();

            entity.Property(product => product.RegularPrice)
                .HasColumnName("regular_price")
                .HasPrecision(18, 2)
                .IsRequired();
        });


        builder.Entity<DiscProduct>(entity =>
        {
            entity.ToTable("DiscProducts");

            entity.HasKey(discProduct => new { discProduct.Sku, discProduct.IdUser });

            entity.Property(discProduct => discProduct.Sku)
                .HasColumnName("sku")
                .HasMaxLength(64)
                .IsRequired();

            entity.Property(discProduct => discProduct.IdUser)
                .HasColumnName("idUser")
                .IsRequired();

            entity.Property(discProduct => discProduct.RegularPrice)
                .HasColumnName("regular_price")
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(discProduct => discProduct.DiscountPercent)
                .HasColumnName("discount_percent")
                .HasPrecision(5, 2)
                .IsRequired();

            entity.Property(discProduct => discProduct.FinalPrice)
                .HasColumnName("final_price")
                .HasPrecision(18, 2)
                .IsRequired();

            entity.HasIndex(discProduct => new { discProduct.Sku, discProduct.IdUser })
                .IsUnique();

            entity.HasOne(discProduct => discProduct.Product)
                .WithMany()
                .HasForeignKey(discProduct => discProduct.Sku)
                .HasPrincipalKey(product => product.Sku)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(discProduct => discProduct.User)
                .WithMany()
                .HasForeignKey(discProduct => discProduct.IdUser)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<IdentityRole>(entity => entity.ToTable("Roles"));
        builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("UserRoles"));
        builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("UserClaims"));
        builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("UserLogins"));
        builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaims"));
        builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("UserTokens"));
    }
}
