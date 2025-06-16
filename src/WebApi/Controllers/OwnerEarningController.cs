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

        if (earningDto == null)
        {
            return BadRequest("Invalid income data.");
        }

        try
        {
            var createdEarning = await _service.CreateOwnerEarningAsync(ownerId, earningDto);
            return CreatedAtAction(nameof(CreateOwnerEarning), new { id = createdEarning.Id }, createdEarning);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
