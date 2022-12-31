using Bogus;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;

namespace MamisSolidarias.WebAPI.Donors.Utils;


internal sealed class DonorBuilder
{
    private static readonly Faker<Donor> Generator = new Faker<Donor>()
        .RuleFor(t => t.Id, f => f.Random.Int())
        .RuleFor(t => t.Name, f => f.Name.FindName())
        .RuleFor(t => t.Email, f => f.Internet.Email())
        .RuleFor(t => t.Phone, f => f.Phone.PhoneNumber("##########"))
        .RuleFor(t => t.IsGodFather, f => f.Random.Bool())
        .RuleFor(t=> t.MercadoPagoEmail, f => f.Internet.Email())
        .RuleFor(t=> t.Dni, f => f.Random.Int(1000000, 99999999).ToString())
        ;    
    private readonly Donor _donor = Generator.Generate();

    private readonly DonorsDbContext? _dbContext;

    public DonorBuilder(DonorsDbContext? db) => _dbContext = db;
    public DonorBuilder(Donor? obj = null) => _donor = obj ?? Generator.Generate();
    

    public Donor Build()
    {
        _dbContext?.Add(_donor);
        _dbContext?.SaveChanges();
        _dbContext?.ChangeTracker.Clear();
        return _donor;
    }

    public DonorBuilder WithId(int id)
    {
        _donor.Id = id;
        return this;
    }

    public DonorBuilder WithName(string name)
    {
        _donor.Name = name;
        return this;
    }
    
    public DonorBuilder WithEmail(string email)
    {
        _donor.Email = email;
        return this;
    }
    
    public DonorBuilder WithPhone(string phone)
    {
        _donor.Phone = phone;
        return this;
    }
    
    public DonorBuilder WithIsGodFather(bool isGodFather)
    {
        _donor.IsGodFather = isGodFather;
        return this;
    }
    
    public DonorBuilder WithMercadoPagoEmail(string? mercadoPagoEmail)
    {
        _donor.MercadoPagoEmail = mercadoPagoEmail;
        return this;
    }
    
    public DonorBuilder WithDni(string? dni)
    {
        _donor.Dni = dni;
        return this;
    }

    public static implicit operator Donor(DonorBuilder b) => b.Build();
    public static implicit operator DonorBuilder(Donor b) => new(b);

}