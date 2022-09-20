using EntityFramework.Exceptions.Common;
using FastEndpoints;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Id.PUT;

internal sealed class Endpoint : Endpoint<Request, Response>
{
    private readonly DbAccess _db;

    public Endpoint(DonorsDbContext dbContext, DbAccess? dbAccess = null)
    {
        _db = dbAccess ?? new DbAccess(dbContext);
    }

    public override void Configure()
    {
        Put("donors/{id}");
        Policies(MamisSolidarias.Utils.Security.Policies.CanWrite);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var donor = await _db.GetDonorAsync(req.Id, ct);
        if (donor is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        try
        {
            donor.Name = req.Name;
            donor.Email = req.Email;
            donor.Phone = req.Phone;
            donor.IsGodFather = req.IsGodFather;

            await _db.SaveChangesAsync(ct);
            await SendOkAsync(new Response(donor.Id,donor.Name,donor.Email,donor.Phone,donor.IsGodFather), ct);
        }
        catch (UniqueConstraintException )
        {
            AddError("Algunos de los datos ingresados ya existen en la base de datos");
            await SendErrorsAsync(cancellation:ct);
        }
    }


}