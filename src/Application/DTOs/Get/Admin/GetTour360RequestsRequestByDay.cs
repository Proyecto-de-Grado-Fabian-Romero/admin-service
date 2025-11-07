using AdminService.Src.Domain.Enums;

namespace AdminService.Src.Application.DTOs.Get.Admin;

public class GetTour360RequestsRequestByDay
{
    public Tour360Status? Status { get; set; }

    public int Page { get; set; } = 1;

    public int Limit { get; set; } = 10;

    public long? ScheduledDayTimestamp { get; set; }
}
