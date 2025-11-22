using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Src.WebApi.Controllers;

[ApiController]
[Route("api/owners/earnings")]
public class OwnerEarningController(IOwnerEarningService service) : ControllerBase
{
    private readonly IOwnerEarningService _service = service;

    [HttpPost]
    public async Task<IActionResult> CreateOwnerEarning([FromBody] CreateOwnerEarningDto earningDto)
    {
        var publicId = Request.Cookies["publicId"];
        if (string.IsNullOrEmpty(publicId))
        {
            return Unauthorized("User not authenticated.");
        }

        var ownerId = Guid.Parse(publicId);
        Console.WriteLine($"Creating earning for OwnerId: {ownerId}");
        Console.WriteLine($"Earning Amount: {earningDto.OwnerId}");

        if (earningDto == null)
        {
            return BadRequest("Invalid income data.");
        }

        try
        {
            var createdEarning = await _service.CreateOwnerEarningAsync(ownerId, earningDto);
            return CreatedAtAction(
                nameof(CreateOwnerEarning),
                new { id = createdEarning.Id },
                createdEarning
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthly([FromQuery] long fromMs, [FromQuery] long toMs)
    {
        var publicId = Request.Cookies["publicId"];
        if (string.IsNullOrEmpty(publicId))
        {
            return Unauthorized("User not authenticated.");
        }

        if (fromMs <= 0 || toMs <= 0 || fromMs >= toMs)
        {
            return BadRequest("fromMs/toMs inválidos. Deben ser ms UTC y fromMs < toMs.");
        }

        var ownerId = Guid.Parse(publicId);

        var result = await _service.GetMonthlyEarningsAsync(ownerId, fromMs, toMs);
        return Ok(result);
    }
}
