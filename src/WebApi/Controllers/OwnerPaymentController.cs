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

    [HttpGet("income")]
    public async Task<IActionResult> GetIncomeList([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var publicId = Request.Cookies["publicId"];
        if (publicId == null)
        {
            return Unauthorized();
        }

        var result = await _service.GetOwnerIncomeListAsync(publicId, page, limit);
        return Ok(result);
    }

    [HttpGet("received")]
    public async Task<IActionResult> GetReceivedPayments([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var publicId = Request.Cookies["publicId"];
        if (publicId == null)
        {
            return Unauthorized();
        }

        var result = await _service.GetOwnerReceivedPaymentsAsync(publicId, page, limit);
        return Ok(result);
    }

    [HttpGet("income/{id}")]
    public async Task<IActionResult> GetIncomeDetails(Guid id)
    {
        var publicId = Request.Cookies["publicId"];
        if (publicId == null)
        {
            return Unauthorized();
        }

        var result = await _service.GetOwnerIncomeDetailsAsync(publicId, id);
        return Ok(result);
    }

    [HttpGet("received/{id}")]
    public async Task<IActionResult> GetReceivedPaymentDetails(Guid id)
    {
        var publicId = Request.Cookies["publicId"];
        if (publicId == null)
        {
            return Unauthorized();
        }

        var result = await _service.GetOwnerReceivedPaymentDetailsAsync(publicId, id);
        return Ok(result);
    }
}
