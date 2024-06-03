using System.Data;
using ENtityFramework.Context;
using ENtityFramework.DTO;
using ENtityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFdatabaseFirst.Services;

public class ClientTripService
{
    private readonly ApbdContext _dbContext;

    public ClientTripService(ApbdContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AssignClientToTrip(AssignClientToTripDto toAssign, int idTrip)
    {
        var client = await GetClientByPeselAsync(toAssign.Pesel);

        if (await CheckIfClientHasTripInArgumentAsync(client, idTrip))
        {
            throw new ClientAlreadyRegisteredForTripException("Client already registered for this trip");
        }
        
        var trip = await GetFutureTripById(idTrip);
        var result = new ClientTrip()
        {
            IdClient = client.IdClient,
            IdClientNavigation = client,
            IdTrip = trip.IdTrip,
            IdTripNavigation = trip,
            PaymentDate = toAssign.PaymentDate,
            RegisteredAt = DateTime.Now
        };
        await _dbContext.ClientTrips.AddAsync(result);
        await _dbContext.SaveChangesAsync();




    }

    public async Task<Trip> GetFutureTripById(int idTrip)
    {
        var res = await _dbContext.Trips.Where(tr => tr.IdTrip == idTrip && tr.DateFrom > DateTime.Now).FirstOrDefaultAsync();
        if (res == null)
        {
            throw new DataException("There is no such trip");
        }
        

        return res;
    }

    public async Task<Client> GetClientByPeselAsync(string pesel)
    {
        var client = await _dbContext.Clients
            .Where(cl =>cl.Pesel.Equals(pesel)).FirstOrDefaultAsync();
        if (client == null)
        {
            throw new Exception("Client not found");
        }
        
        return client;
    }

    public async Task<bool> CheckIfClientHasTripInArgumentAsync(Client client, int idTrip)
    {
        var result = await _dbContext
                                .ClientTrips
                                .Where(trip => trip.IdClient == client.IdClient && trip.IdTrip == idTrip)
                                .AnyAsync();
        
        return result;
    }
}