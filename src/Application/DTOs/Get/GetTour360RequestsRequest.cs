using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Get;

public class GetTour360RequestsRequest
{
    public Tour360Status? Status { get; set; }

    public int Page { get; set; } = 1;

    public int Limit { get; set; } = 10;
}
