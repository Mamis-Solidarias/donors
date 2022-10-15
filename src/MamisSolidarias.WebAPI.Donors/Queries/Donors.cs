using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using MamisSolidarias.Infrastructure.Donors;
using MamisSolidarias.Infrastructure.Donors.Models;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Donors.Queries;

public class Donors
{
    
    public record DonorFilters(bool? IsGodFather, int? OwnerId, string? Name);

    [Authorize(Policy = "CanRead")]
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Donor> GetDonors(DonorsDbContext dbContext, DonorFilters? filters = null)
    {
        var query = dbContext.Donors.AsNoTracking().AsQueryable();
        if (filters is null)
            return query;

        if (filters.IsGodFather is not null)
            query = query.Where(t => t.IsGodFather == filters.IsGodFather);
        
        if (filters.OwnerId is not null)
            query = query.Where(t => t.CreatedBy == filters.OwnerId);
        
        if (filters.Name is not null)
            query = query.Where(t => t.Name.ToLower().Contains(filters.Name.ToLower()));

        return query;
    }
       
    
    [Authorize(Policy = "CanRead")]
    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<Donor> GetDonor(DonorsDbContext dbContext, int? id) =>
        dbContext.Donors.Where(d => d.Id == id);
}