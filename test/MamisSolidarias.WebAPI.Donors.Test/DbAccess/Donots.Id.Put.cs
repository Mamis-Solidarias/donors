using System.Threading.Tasks;
using EntityFramework.Exceptions.Sqlite;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;
using MamisSolidarias.WebAPI.Donors.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Donors.DbAccess;

internal sealed class DonotsIdPut
{
    private DonorsDbContext _dbContext = null!;
    private Endpoints.Donors.Id.PUT.DbAccess _dbAccess = null!;
    private DataFactory _dataFactory = null!;

    [SetUp]
    public void TestWithSqlite()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<DonorsDbContext>()
            .UseSqlite(connection)
            .UseExceptionProcessor()
            .Options;

        _dbContext = new DonorsDbContext(options);
        _dbContext.Database.EnsureCreated();

        _dbAccess = new(_dbContext);
        _dataFactory = new(_dbContext);
    }

    [TearDown]
    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Test]
    public async Task DonorExists_ReturnsObject()
    {
        // Arrange
        Donor donor = _dataFactory.GenerateDonor();
        
        // Act
        var result = await _dbAccess.GetDonorAsync(donor.Id, default);
        
        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(donor.Email);
        result.Name.Should().Be(donor.Name);
        result.Phone.Should().Be(donor.Phone);
        result.IsGodFather.Should().Be(donor.IsGodFather);
        result.Id.Should().Be(donor.Id);
    }

    [Test]
    public async Task DonorDoesNotExists_ReturnsNull()
    {
        // Arrange
        const int id = 123;
        
        // Act
        var result = await _dbAccess.GetDonorAsync(id, default);
        
        // Assert
        result.Should().BeNull();
    }
}