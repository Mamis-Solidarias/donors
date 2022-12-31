using FastEndpoints;
using FluentValidation;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors.POST;

public class Request
{
    /// <summary>
    /// Name of the Donor
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Email of the donor. Optional
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Email used in Mercado Pago. It may be different from the contact email. Optional
    /// </summary>
    public string? MercadoPagoEmail { get; set; }
    
    /// <summary>
    /// National ID number. Optional
    /// </summary>
    public string? Dni { get; set; }
    
    /// <summary>
    /// Phone of the donor. Optional
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Whether the sponsor is a godfather or not
    /// </summary>
    public bool IsGodFather { get; set; }
    
    [FromClaim("Id")]
    public int UserId { get; set; }
}

internal class RequestValidator : Validator<Request>
{
    public RequestValidator()
    {
        RuleFor(t=> t.Name)
            .NotEmpty().WithMessage("Es obligatorio indicar el nombre del donante")
            .MaximumLength(100).WithMessage("El nombre del donante no puede tener más de 100 caracteres");
        
        RuleFor(t=> t.Email)
            .EmailAddress()
                .When(t=> t.Email is not null).WithMessage("El email no tiene un formato válido")
            .MaximumLength(200)
                .When(t=> t.Email is not null).WithMessage("El email no puede tener más de 200 caracteres")
            .NotEmpty()
                .When(t=> t.Phone is null).WithMessage("Es obligatorio indicar el email o el teléfono del donante");
        
        RuleFor(t=> t.Phone)
            .Matches(@"^\+54\d{8,12}$")
                .When(t=> t.Phone is not null).WithMessage("El teléfono no tiene un formato válido")
            .MaximumLength(15)
                .When(t=> t.Phone is not null).WithMessage("El teléfono no puede tener más de 15 caracteres")
            .NotEmpty()
                .When(t=> t.Email is null).WithMessage("Es obligatorio indicar el email o el teléfono del donante");
        
        RuleFor(t=> t.UserId)
            .NotEmpty().WithMessage("Es obligatorio indicar el usuario que realiza la acción");
        
        RuleFor(t=> t.MercadoPagoEmail)
            .EmailAddress()
                .When(t=> t.MercadoPagoEmail is not null).WithMessage("El email de Mercado Pago no tiene un formato válido")
            .MaximumLength(200)
                .When(t=> t.MercadoPagoEmail is not null).WithMessage("El email de Mercado Pago no puede tener más de 200 caracteres");

        RuleFor(t => t.Dni)
            .Matches(@"^\d{7,8}$")
            .When(t => t.Dni is not null).WithMessage("El DNI no tiene un formato válido");
    }
}