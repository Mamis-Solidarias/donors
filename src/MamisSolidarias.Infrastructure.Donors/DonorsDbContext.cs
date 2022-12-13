using MamisSolidarias.Infrastructure.Donors.Models;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618

namespace MamisSolidarias.Infrastructure.Donors;

public class DonorsDbContext: DbContext
{
    public DbSet<Donor> Donors { get; set; }

    public DonorsDbContext(DbContextOptions<DonorsDbContext> options) : base(options)
    {
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new DonorConfigurator().Configure(modelBuilder.Entity<Donor>());
    }
    
}