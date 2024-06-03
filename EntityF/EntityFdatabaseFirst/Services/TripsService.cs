using ENtityFramework.Context;
using ENtityFramework.DTO;
using ENtityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace ENtityFramework.Services;

public class TripsService
{
    private readonly ApbdContext _dbContext;

    public TripsService(ApbdContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Object> GetPageTripsAsync(int pageSize, int page)
    {
        int pages = (int)Math.Ceiling(await _dbContext.Trips.CountAsync()/ (double)pageSize);

        int toSkip = (page - 1) * pageSize;
        
        
        var trips = await _dbContext.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .OrderByDescending(t => t.DateFrom)
            .Skip(toSkip)
            .Take(pageSize)
            .ToListAsync();
        var result =  new
        {
            pageNum = page,
            pageSize,
            allPages = pages,
            trips = trips.Select(t => new TripDto
            {
                IdTrip = t.IdTrip,
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries.Select(country => new CountryDto()
                {
                    Name = country.Name
                } ).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientTripDto
                {
                    IdClient = ct.IdClient,
                    FirstName = ct.IdClientNavigation.FirstName,
                    LastName = ct.IdClientNavigation.LastName,
                }).ToList()
            }).ToList()
        };
        return result;

    }

    public async Task<List<TripDto>> GetAllTripsAsync()
    {
        var trips = await _dbContext.Trips
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.IdClientNavigation)
            .Include(t => t.IdCountries)
            .ToListAsync();


        var result =  trips.Select(t => new TripDto
        {
            IdTrip = t.IdTrip,
            Name = t.Name,
            Description = t.Description,
            DateFrom = t.DateFrom,
            DateTo = t.DateTo,
            MaxPeople = t.MaxPeople,
            Countries = t.IdCountries.Select(country => new CountryDto()
            {
                Name = country.Name
            } ).ToList(),
            Clients = t.ClientTrips.Select(ct => new ClientTripDto
            {
                IdClient = ct.IdClient,
                FirstName = ct.IdClientNavigation.FirstName,
                LastName = ct.IdClientNavigation.LastName,
            }).ToList()
        }).ToList();
        
        
        return result;
    }
}