using EntityFdatabaseFirst;
using EntityFdatabaseFirst.Services;
using ENtityFramework.DTO;
using ENtityFramework.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ENtityFramework.Controllers;

[ApiController]
[Route("trips")]
public class TripsController : ControllerBase
{
    private readonly TripsService _tripsService;
    private readonly ClientTripService _clientTripService;

    public TripsController(TripsService tripsService, ClientTripService clientTripService)
    {
        _tripsService = tripsService;
        _clientTripService = clientTripService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTrips(int page = 1, int pageSize = 10)
    {
        var result = await _tripsService.GetPageTripsAsync(pageSize, page);
        
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, AssignClientToTripDto assignClientToTripDto)
    {
        try
        {
            await _clientTripService.AssignClientToTrip(assignClientToTripDto, idTrip);
            return Ok();
        }
        catch (ClientAlreadyRegisteredForTripException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

}