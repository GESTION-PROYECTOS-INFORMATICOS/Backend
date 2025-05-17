using backGestion.Models;
using backGestion.Services;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/[controller]")]
public class MallaController : ControllerBase
{
    private readonly IMallaService _mallaService;

    public MallaController(IMallaService mallaService)
    {
        _mallaService = mallaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Malla>>> getAllMallas()
    {
        var mallas = await _mallaService.GetAllMallas();
        return Ok(new {mallas});
    }
}