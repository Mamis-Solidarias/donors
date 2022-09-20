using System.Threading.Tasks;
using EntityFramework.Exceptions.Common;
using EntityFramework.Exceptions.Sqlite;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;
using MamisSolidarias.WebAPI.Donors.Utils;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Donors.DbAccess;

internal sealed class DonorsPost
{
    private DonorsDbContext _dbContext = null!;
    private Endpoints.Donors.POST.DbAccess _dbAccess = null!;
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
    public async Task ValidDonor_ShouldBeAdded()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor().WithId(0);
        
        // Act
        await _dbAccess.CreateDonor(donor, default);
        
        // Assert
        donor.Id.Should().BePositive();
    }

    [Test]
    public async Task InvalidDonor_RepeatedName_ShouldThrowUniqueConstraint()
    {
        // Arrange
        Donor ogDonor = _dataFactory.GenerateDonor();
        Donor newDonor = DataFactory.GetDonor().WithName(ogDonor.Name);
        
        // Act
        var action = async () => await _dbAccess.CreateDonor(newDonor, default);
        
        // Assert
        await action.Should().ThrowAsync<UniqueConstraintException>();
    }

    [Test]
    public async Task InvalidDonor_RepeatedEmail_ShouldThrowUniqueConstraint()
    {
        // Arrange
        const string email = "test@mail.com";
        Donor ogDonor = _dataFactory.GenerateDonor().WithEmail(email);
        Donor newDonor = DataFactory.GetDonor().WithEmail(email);
        
        // Act
        var action = async () => await _dbAccess.CreateDonor(newDonor, default);
        
        // Assert
        await action.Should().ThrowAsync<UniqueConstraintException>();
    }

    [Test]
    public async Task InvalidDonor_RepeatedPhone_ShouldThrowUniqueConstraint()
    {
        // Arrange
        const string phone = "+5491132112333";
        Donor ogDonor = _dataFactory.GenerateDonor().WithPhone(phone);
        Donor newDonor = DataFactory.GetDonor().WithPhone(phone);
        
        // Act
        var action = async () => await _dbAccess.CreateDonor(newDonor, default);
        
        // Assert
        await action.Should().ThrowAsync<UniqueConstraintException>();
    }
}