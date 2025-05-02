using AdminService.Src.Application.DTOs;
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
        return Ok(new
        {
            items,
            totalItems,
            totalPages = (int)Math.Ceiling((double)totalItems / request.Limit),
        });
    }
}
