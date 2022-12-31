using FastEndpoints;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Id.PUT;

internal sealed class Description : Summary<Endpoint>
{
    public Description()
    { 
        Summary = "It updates an existing donor";
        ExampleRequest = new Request
        {
            Email = "test@mail.com",
            IsGodFather = true,
            Name = "Carlos",
            Phone = "+5491234567890",
            Dni = "50123321",
            MercadoPagoEmail = "mp@gmail.com"
        };
        
        Response<Response>(201,"The donor was created successfully");
        Response(400,"The request is invalid");
        Response(500,"An error occurred while processing the request");
        Response(401,"The user is not authorized to perform this action");
        Response(403,"The user is not allowed to perform this action");
    }
}