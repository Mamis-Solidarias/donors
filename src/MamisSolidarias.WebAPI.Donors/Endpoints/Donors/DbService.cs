using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors;

internal class DbService
{
    private readonly DonorsDbContext? _dbContext;

    public DbService() { }
    public DbService(DonorsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task CreateDonor(Donor donor, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        await _dbContext.Donors.AddAsync(donor, ct);
        await _dbContext.SaveChangesAsync(ct);
    }
}