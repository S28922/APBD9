using ENtityFramework.Context;
using ENtityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ENtityFramework.Services;

public class ClientService
{
    private readonly ApbdContext _dbContext;

    public ClientService(ApbdContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task KickClientAsync(int idClient)
    {
        var client = await GetClientById(idClient); 
        if (client == null)
        {
            throw new Exception("Client not found");
        }

        if (CheckIfClientHaveTrips(idClient))
        {
            throw new InvalidDataException("Client has trips, it's impossible to delete him");
        }

        _dbContext.Clients.Remove(client);
        await _dbContext.SaveChangesAsync();

    }

    public bool CheckIfClientHaveTrips(int idClient)
    {
        var done = _dbContext.ClientTrips.Any(trip => trip.IdClient == idClient);
        
        return done;
    }

    public async Task<Client> GetClientById(int idClient)
    {
        var client = await _dbContext.Clients.FirstOrDefaultAsync(client => client.IdClient == idClient);
        

        return client;

    }

}