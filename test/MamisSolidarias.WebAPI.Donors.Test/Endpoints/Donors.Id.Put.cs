using System.Threading;
using System.Threading.Tasks;
using EntityFramework.Exceptions.Common;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Donors.Models;
using MamisSolidarias.Utils.Test;
using MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Id.PUT;
using MamisSolidarias.WebAPI.Donors.Utils;
using Moq;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Donors.Endpoints;

internal sealed class DonorsIdPut
{
    private Endpoint _endpoint = null!;
    private readonly Mock<Donors.Id.PUT.DbAccess> _mockDb = new();

    [SetUp]
    public void Setup()
    {
        _endpoint = EndpointFactory.CreateEndpoint<Endpoint>(null, _mockDb.Object);
    }

    [TearDown]
    public void Teardown()
    {
        _mockDb.Reset();
    }

    [Test]
    public async Task ValidDonor_ReturnsOk()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor();
        _mockDb.Setup(t =>
            t.GetDonorAsync(It.Is<int>(r => r == donor.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync(donor);

        var request = new Request
        {
            Id = donor.Id,
            Name = "New Name",
            Email = "new@mail.com",
            Phone = "12456789",
            IsGodFather = false,
            Dni = "50123321",
            MercadoPagoEmail = "mp@gmail.com"
        };

        // Act
        await _endpoint.HandleAsync(request, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
        _endpoint.Response.Id.Should().Be(donor.Id);
        _endpoint.Response.Email.Should().Be(request.Email);
        _endpoint.Response.Phone.Should().Be($"+549{request.Phone}");
        _endpoint.Response.Name.Should().Be(request.Name);
        _endpoint.Response.IsGodFather.Should().Be(request.IsGodFather);
        _endpoint.Response.Dni.Should().Be(request.Dni);
        _endpoint.Response.MercadoPagoEmail.Should().Be(request.MercadoPagoEmail);
    }

    [Test]
    public async Task InvalidDonor_RepeatedEmail_ReturnsError()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor();
        _mockDb.Setup(t =>
            t.GetDonorAsync(It.Is<int>(r => r == donor.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync(donor);

        _mockDb.Setup(t =>
            t.SaveChangesAsync(It.IsAny<CancellationToken>())
        ).ThrowsAsync(new UniqueConstraintException());

        var request = new Request
        {
            Id = donor.Id,
            Name = "New Name",
            Email = "repeated@mail.com",
            Phone = "+5412456789",
            IsGodFather = false
        };

        // Act
        await _endpoint.HandleAsync(request, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }
    
    [Test]
    public async Task InvalidDonor_RepeatedName_ReturnsError()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor();
        _mockDb.Setup(t =>
            t.GetDonorAsync(It.Is<int>(r => r == donor.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync(donor);

        _mockDb.Setup(t =>
            t.SaveChangesAsync(It.IsAny<CancellationToken>())
        ).ThrowsAsync(new UniqueConstraintException());

        var request = new Request
        {
            Id = donor.Id,
            Name = "Repeated Name",
            Email = "new@mail.com",
            Phone = "+5412456789",
            IsGodFather = false
        };

        // Act
        await _endpoint.HandleAsync(request, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }
    
    [Test]
    public async Task InvalidDonor_RepeatedPhone_ReturnsError()
    {
        // Arrange
        Donor donor = DataFactory.GetDonor();
        _mockDb.Setup(t =>
            t.GetDonorAsync(It.Is<int>(r => r == donor.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync(donor);

        _mockDb.Setup(t =>
            t.SaveChangesAsync(It.IsAny<CancellationToken>())
        ).ThrowsAsync(new UniqueConstraintException());

        var request = new Request
        {
            Id = donor.Id,
            Name = "New Name",
            Email = "new@mail.com",
            Phone = "+5412456789",
            IsGodFather = false
        };

        // Act
        await _endpoint.HandleAsync(request, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }
}