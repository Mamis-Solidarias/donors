using MamisSolidarias.Infrastructure.Donors;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Test;

internal class DbService
{
    private readonly DonorsDbContext? _dbContext;

    public DbService() { }
    public DbService(DonorsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
}