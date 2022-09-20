using System.Collections.Generic;
using System.Linq;
using Bogus;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;

namespace MamisSolidarias.WebAPI.Donors.Utils;


internal sealed class DataFactory
{
    private readonly DonorsDbContext? _dbContext;
    public DataFactory(DonorsDbContext? dbContext)
    {
        _dbContext = dbContext;
    }
    
    public static DonorBuilder GetDonor() => new();
    public DonorBuilder GenerateDonor() => new(_dbContext);

    public static IEnumerable<DonorBuilder> GetDonors(int n) => Enumerable.Range(0, n).Select(_ => GetDonor());
    public IEnumerable<DonorBuilder> GenerateDonors(int n) => Enumerable.Range(0, n).Select(_ => GenerateDonor());
}