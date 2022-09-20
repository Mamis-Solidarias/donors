using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors.POST;

internal class DbAccess
{
    private readonly DonorsDbContext? _dbContext;

    public DbAccess() { }
    public DbAccess(DonorsDbContext dbContext)
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