using EntityFramework.Exceptions.Common;
using FastEndpoints;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.WebAPI.Donors.Extensions;

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
        Policies(Utils.Security.Policies.CanWrite);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        try
        {
            var donor = await _db.GetDonorAsync(req.Id, ct);
            if (donor is null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            donor.Name = req.Name.PrepareForDb().Capitalize()!;
            donor.Email = req.Email.PrepareForDb();
            donor.Phone = req.Phone.ParsePhoneNumber();
            donor.IsGodFather = req.IsGodFather;
            donor.MercadoPagoEmail = req.MercadoPagoEmail.PrepareForDb();
            donor.Dni = req.Dni.PrepareForDb();

            await _db.SaveChangesAsync(ct);
            await SendOkAsync(new Response(donor.Id, donor.Dni,donor.Name, donor.Email,donor.MercadoPagoEmail, donor.Phone, donor.IsGodFather), ct);
        }
        catch (UniqueConstraintException)
        {
            AddError("Algunos de los datos ingresados ya existen en la base de datos");
            await SendErrorsAsync(cancellation: ct);
        }
    }
}