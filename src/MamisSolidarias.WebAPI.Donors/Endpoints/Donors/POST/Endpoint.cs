using EntityFramework.Exceptions.Common;
using FastEndpoints;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;
using MamisSolidarias.WebAPI.Donors.Extensions;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors.POST;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly DbAccess _db;

    public Endpoint(DonorsDbContext dbContext, DbAccess? db = null)
    {
        _db = db ?? new DbAccess(dbContext);
    }

    public override void Configure()
    {
        Post("donors");
        Policies(Utils.Security.Policies.CanWrite);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            var donor = Map(req);
            await _db.CreateDonor(donor, ct);
            await SendAsync(new Response(donor.Id), 201, ct);
        }
        catch (UniqueConstraintException e)
        {
            AddError("Algunos de los datos ingresados (Nombre, telefono o mail) ya existen en la base de datos");
            await SendErrorsAsync(cancellation: ct);
        }
    }

    private static Donor Map(Request req)
    {
        return new Donor
        {
            Email = req.Email?.PrepareForDb(),
            Name = req.Name.PrepareForDb().Capitalize(),
            Phone = req.Phone?.PrepareForDb(),
            IsGodFather = req.IsGodFather
        };
    }
}