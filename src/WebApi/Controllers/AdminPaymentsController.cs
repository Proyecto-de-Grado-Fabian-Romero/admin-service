using AdminService.Src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Src.WebApi.Controllers;

[ApiController]
[Route("api/admin/payments")]
public class AdminPaymentsController : ControllerBase
{
    private readonly IAdminPaymentService _paymentService;

    public AdminPaymentsController(IAdminPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPayments([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var result = await _paymentService.GetPaymentsAsync(page, limit);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentDetails(Guid id)
    {
        var result = await _paymentService.GetPaymentDetailsAsync(id);
        return Ok(result);
    }
}
