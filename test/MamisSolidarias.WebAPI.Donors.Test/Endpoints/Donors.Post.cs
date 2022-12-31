
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.Exceptions.Common;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Donors.Models;
using MamisSolidarias.Utils.Test;
using MamisSolidarias.WebAPI.Donors.Endpoints.Donors.POST;
using MamisSolidarias.WebAPI.Donors.Utils;
using Moq;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Donors.Endpoints;

internal sealed class DonorsPostTest
{
    private readonly Mock<Donors.POST.DbAccess> _mockDbService = new();
    private Endpoint _endpoint = null!;

    [SetUp]
    public void Setup()
    {
        _endpoint = EndpointFactory
            .CreateEndpoint<Endpoint>(null, _mockDbService.Object);
    }

    [Test]
    public async Task ValidDonor_ShouldReturnCreated()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor().WithId(0);
        _mockDbService.Setup(t => t.CreateDonor(
                It.Is<Donor>(r => r.Name == donor.Name),
                It.IsAny<CancellationToken>()
            )
        );

        var req = new Request
        {
            Email = donor.Email,
            Name = donor.Name,
            IsGodFather = donor.IsGodFather,
            Phone = donor.Phone,
            UserId = 123,
            Dni = "50123321",
            MercadoPagoEmail = "mp@gmail.com"
        };

        // Act
        await _endpoint.HandleAsync(req, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(201);
        _endpoint.Response.Id.Should().Be(donor.Id);
    }
    [Test]
    public async Task ValidDonor_WithOnlyEmail_ShouldReturnCreated()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor().WithId(0);
        _mockDbService.Setup(t => t.CreateDonor(
                It.Is<Donor>(r => r.Name == donor.Name),
                It.IsAny<CancellationToken>()
            )
        );

        var req = new Request
        {
            Email = donor.Email,
            Name = donor.Name,
            IsGodFather = donor.IsGodFather,
            UserId = 123
        };

        // Act
        await _endpoint.HandleAsync(req, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(201);
        _endpoint.Response.Id.Should().Be(donor.Id);
    }
    
    [Test]
    public async Task ValidDonor_WithOnlyPhone_ShouldReturnCreated()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor().WithId(0);
        _mockDbService.Setup(t => t.CreateDonor(
                It.Is<Donor>(r => r.Name == donor.Name),
                It.IsAny<CancellationToken>()
            )
        );

        var req = new Request
        {
            Name = donor.Name,
            IsGodFather = donor.IsGodFather,
            Phone = donor.Phone,
            UserId = 123,
            Dni = "50123321",
            MercadoPagoEmail = "mp@gmail.com"
        };

        // Act
        await _endpoint.HandleAsync(req, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(201);
        _endpoint.Response.Id.Should().Be(donor.Id);
    }
    
    [Test]
    public async Task InvalidDonor_RepeatedEmail_ShouldReturnError()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor().WithId(0);
        _mockDbService.Setup(t => t.CreateDonor(
                It.Is<Donor>(r => r.Email == donor.Email!.ToLowerInvariant()),
                It.IsAny<CancellationToken>()
            )
        ).ThrowsAsync(new UniqueConstraintException());

        var req = new Request
        {
            Email = donor.Email,
            Name = donor.Name,
            IsGodFather = donor.IsGodFather,
            Phone = donor.Phone,
            UserId = 123
        };

        // Act
        await _endpoint.HandleAsync(req, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }
    
    [Test]
    public async Task InvalidDonor_RepeatedPhone_ShouldReturnError()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor().WithId(0);
        
        _mockDbService.Setup(t => t.CreateDonor(
                It.Is<Donor>(r => r.Phone == donor.Phone),
                It.IsAny<CancellationToken>()
            )
        ).ThrowsAsync(new UniqueConstraintException());

        var req = new Request
        {
            Email = donor.Email,
            Name = donor.Name,
            IsGodFather = donor.IsGodFather,
            Phone = donor.Phone,
            UserId = 123,
            Dni = "50123321",
            MercadoPagoEmail = "mp@gmail.com"
        };

        // Act
        await _endpoint.HandleAsync(req, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }
}