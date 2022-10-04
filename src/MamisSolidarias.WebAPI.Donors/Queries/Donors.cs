using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;
using Microsoft.AspNetCore.Mvc;

namespace MamisSolidarias.WebAPI.Donors.Queries;

public class Donors
{
    [Authorize(Policy = "CanRead")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Donor> GetDonors([FromServices] DonorsDbContext dbContext) =>
        dbContext.Donors;
}