using AdminService.Src.Application.DTOs.Update;
using AdminService.Src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Src.WebApi.Controllers;

[ApiController]
[Route("api/admin/debts")]
public class AdminDebtsController(IAdminDebtService debtService) : ControllerBase
{
    private readonly IAdminDebtService _debtService = debtService;

    [HttpGet]
    public async Task<IActionResult> GetAllDebts([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        var result = await _debtService.GetOutstandingDebtsAsync(page, limit);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDebtDetails(Guid id)
    {
        var result = await _debtService.GetDebtDetailsAsync(id);
        return Ok(result);
    }

    [HttpPatch("{debtId}/mark-as-paid")]
    public async Task<IActionResult> MarkDebtAsPaid(Guid debtId, [FromBody] MarkDebtAsPaidRequest request)
    {
        try
        {
            var payment = await _debtService.MarkDebtAsPaidAsync(debtId, request.Reference);
            return Ok(new { message = "Debt marked as paid and payment recorded.", payment });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
