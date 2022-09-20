using EntityFramework.Exceptions.Common;
using FastEndpoints;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly DbService _db;

    public Endpoint(DonorsDbContext dbContext, DbService? db = null)
    {
        _db = db ?? new DbService(dbContext);
    }

    public override void Configure()
    {
        Post("donors");
        Policies(Utils.Security.Policies.CanWrite);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var donor = Map(req);
        try
        {
            await _db.CreateDonor(donor, ct);
            await SendAsync(new Response(donor.Id), 201, ct);
        }
        catch (UniqueConstraintException e)
        {
            AddError("Algunos de los datos ingresados (Nombre, telefono o mail) ya existen en la base de datos");
            await SendErrorsAsync(cancellation: ct);
        }
        catch (MaxLengthExceededException e)
        {
            AddError("Algunos de los datos ingresados exceden el tamaño máximo permitido");
            await SendErrorsAsync(cancellation: ct);
        }
    }

    private static Donor Map(Request req)
    {
        return new Donor
        {
            Email = req.Email,
            Name = req.Name,
            Phone = req.Phone,
            IsGodFather = req.IsGodFather
        };
    }
}