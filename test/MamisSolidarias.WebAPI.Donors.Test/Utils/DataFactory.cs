using System.Collections.Generic;
using System.Linq;
using MamisSolidarias.Infrastructure.Donors;

namespace MamisSolidarias.WebAPI.Donors.Utils;

internal sealed class DataFactory
{
    private readonly DonorsDbContext? _dbContext;

    public DataFactory(DonorsDbContext? dbContext)
    {
        _dbContext = dbContext;
    }

    public static DonorBuilder GetDonor()
    {
        return new();
    }

    public DonorBuilder GenerateDonor()
    {
        return new(_dbContext);
    }

    public static IEnumerable<DonorBuilder> GetDonors(int n)
    {
        return Enumerable.Range(0, n).Select(_ => GetDonor());
    }

    public IEnumerable<DonorBuilder> GenerateDonors(int n)
    {
        return Enumerable.Range(0, n).Select(_ => GenerateDonor());
    }
}