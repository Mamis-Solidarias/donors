using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MamisSolidarias.Infrastructure.Donors.Models;

public class Donor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool IsGodFather { get; set; }
    public int CreatedBy { get; set; }
}

internal class DonorCondfigurator : IEntityTypeConfiguration<Donor>
{
    public void Configure(EntityTypeBuilder<Donor> builder)
    {
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(t => t.Name)
            .IsUnique();
        
        builder.Property(t => t.Email)
            .HasMaxLength(100);

        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.Property(t => t.Phone)
            .HasMaxLength(15);
        
        builder.HasIndex(t => t.Phone)
            .IsUnique();

        builder.Property(t => t.IsGodFather)
            .IsRequired();

        builder.Property(t => t.CreatedBy)
            .IsRequired();
    }
}