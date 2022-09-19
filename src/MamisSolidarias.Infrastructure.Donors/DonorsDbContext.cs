using MamisSolidarias.Infrastructure.Donors.Models;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.Infrastructure.Donors;

public class DonorsDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DonorsDbContext(DbContextOptions<DonorsDbContext> options) : base(options)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(
            model =>
            {
                model.HasKey(t => t.Id);
                model.Property(t => t.Id).ValueGeneratedOnAdd();
                model.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(500);

            }
        );



    }
    
}