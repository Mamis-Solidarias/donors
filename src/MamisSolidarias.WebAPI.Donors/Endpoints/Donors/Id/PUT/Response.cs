namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors.Id.PUT;

/// <summary>
/// Response model
/// </summary>
/// <param name="Id">ID of the donor</param>
/// <param name="Name">Name of the donor</param>
/// <param name="Email">Email of the donor</param>
/// <param name="Phone">Phone number of the donor</param>
/// <param name="IsGodFather">Is the donor a godfather</param>
public sealed record Response(int Id, string? Dni, string Name, string? Email, string? MercadoPagoEmail, string? Phone, bool IsGodFather);