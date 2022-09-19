using FastEndpoints;
using FluentValidation;

namespace MamisSolidarias.WebAPI.Donors.Endpoints.Donors;

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
    /// Phone of the donor. Optional
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    /// Whether the sponsor is a godfather or not
    /// </summary>
    public bool IsGodFather { get; set; }
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
            .MaximumLength(100)
                .When(t=> t.Email is not null).WithMessage("El email no puede tener más de 100 caracteres")
            .NotEmpty()
                .When(t=> t.Phone is null).WithMessage("Es obligatorio indicar el email o el teléfono del donante");
        
        RuleFor(t=> t.Phone)
            .Matches(@"^\+54\d{8,12}$")
                .When(t=> t.Phone is not null).WithMessage("El teléfono no tiene un formato válido")
            .MaximumLength(15)
                .When(t=> t.Phone is not null).WithMessage("El teléfono no puede tener más de 15 caracteres")
            .NotEmpty()
                .When(t=> t.Email is null).WithMessage("Es obligatorio indicar el email o el teléfono del donante");
    }
}