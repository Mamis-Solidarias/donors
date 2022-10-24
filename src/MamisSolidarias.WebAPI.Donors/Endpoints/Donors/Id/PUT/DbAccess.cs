using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Id.PUT;

internal class DbAccess
{
    private readonly DonorsDbContext? _dbContext;

    public DbAccess()
    {
    }

    public DbAccess(DonorsDbContext? dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual Task<Donor?> GetDonorAsync(int id, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        return _dbContext.Donors.AsTracking().FirstOrDefaultAsync(t=> t.Id == id, ct);
    }

    public virtual async Task SaveChangesAsync(CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        await _dbContext.SaveChangesAsync(ct);
    }
}