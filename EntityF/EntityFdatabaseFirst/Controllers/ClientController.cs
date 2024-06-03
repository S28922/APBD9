using ENtityFramework.Services;
using Microsoft.AspNetCore.Mvc;

namespace ENtityFramework.Controllers;

[ApiController]
[Route("clients")]
public class ClientController : ControllerBase
{
    private readonly ClientService _clientService;

    public ClientController(ClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> RemoveClient(int idClient)
    {
        try
        {
            await _clientService.KickClientAsync(idClient);
            return Ok();
        }
        catch (InvalidDataException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
        
    }
}