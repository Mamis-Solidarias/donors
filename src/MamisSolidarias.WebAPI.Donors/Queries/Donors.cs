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
    
    [Authorize(Policy = "CanRead")]
    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<Donor> GetDonor([FromServices] DonorsDbContext dbContext, int id) =>
        dbContext.Donors.Where(d => d.Id == id);
}