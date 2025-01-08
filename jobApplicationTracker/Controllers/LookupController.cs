using jobApplicationTrackerApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jobApplicationTrackerApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class LookupController: ControllerBase
{
    private readonly JobAppTrackerDbContext _context;

    public LookupController(JobAppTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet("statuses")]
    public async Task<IActionResult> GetStatusAsync()
    {
        var statuses = await _context.Statuses.ToListAsync();
        return Ok(statuses);
    }

    [HttpGet("contract-types")]
    public async Task<IActionResult> GetContractTypesAsync()
    {
        var contractTypes = await _context.ContractTypes.ToListAsync();
        return Ok(contractTypes);
    }
}