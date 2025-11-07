using AdminService.Src.Application.DTOs.Create;
using AdminService.Src.Application.DTOs.Get;
using AdminService.Src.Application.DTOs.Get.Admin;
using AdminService.Src.Application.DTOs.Update;
using AdminService.Src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.src.WebApi.Controllers;

[ApiController]
[Route("api/tour360requests")]
public class Tour360RequestController(ITour360RequestService service) : ControllerBase
{
    private readonly ITour360RequestService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetTour360RequestsRequest request)
    {
        var (items, totalItems) = await _service.GetAllAsync(request);
        return Ok(
            new
            {
                items,
                totalItems,
                totalPages = (int)Math.Ceiling((double)totalItems / request.Limit),
            }
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Tour360RequestCreateDto request)
    {
        var authenticatedUserId = Request.Cookies["publicId"];
        if (authenticatedUserId == null)
        {
            return Unauthorized();
        }

        var created = await _service.CreateAsync(request, authenticatedUserId);
        return CreatedAtAction(nameof(Create), new { id = created.EnvironmentId }, created);
    }

    [HttpPut("{id}/upload")]
    public async Task<IActionResult> UploadTour(Guid id, [FromBody] TourUploadDto uploadDto)
    {
        await _service.Upload360TourAsync(id, uploadDto);
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTour360StatusDto dto)
    {
        await _service.UpdateStatusAsync(id, dto.Status);
        return NoContent();
    }

    [HttpPatch("{id}/schedule")]
    public async Task<IActionResult> UpdateSchedule(
        Guid id,
        [FromBody] UpdateTour360ScheduleDto dto
    )
    {
        var ok = await _service.UpdateScheduledDateAsync(id, dto.ScheduledDate);
        if (!ok)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("environments/{environmentPublicId}/last-request-date")]
    public async Task<IActionResult> GetLastRequestDateNonCancelledByEnvironment(
        Guid environmentPublicId
    )
    {
        var last = await _service.GetLastRequestDateNonCancelledByEnvironmentAsync(
            environmentPublicId
        );
        if (last is null)
        {
            return NoContent();
        }

        return Ok(new { requestDate = last });
    }

    [HttpGet("by-day")]
    public async Task<IActionResult> GetAllByDay([FromQuery] GetTour360RequestsRequestByDay request)
    {
        if (!request.ScheduledDayTimestamp.HasValue)
        {
            return BadRequest("ScheduledDayTimestamp is required");
        }

        var (items, totalItems) = await _service.GetAllByDayAsync(request);
        return Ok(
            new
            {
                items,
                totalItems,
                totalPages = (int)Math.Ceiling((double)totalItems / request.Limit),
            }
        );
    }
}
