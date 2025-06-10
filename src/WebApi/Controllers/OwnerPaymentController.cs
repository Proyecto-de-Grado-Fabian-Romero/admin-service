using AdminService.Src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.src.WebApi.Controllers;

[ApiController]
[Route("api/owners/payments")]
public class OwnerPaymentsController(IOwnerPaymentService service) : ControllerBase
{
    private readonly IOwnerPaymentService _service = service;

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var ownerPublicId = Request.Cookies["publicId"];
        if (ownerPublicId == null)
        {
            return Unauthorized();
        }

        var result = await _service.GetOwnerPaymentSummaryAsync(ownerPublicId);
        return Ok(result);
    }
}
