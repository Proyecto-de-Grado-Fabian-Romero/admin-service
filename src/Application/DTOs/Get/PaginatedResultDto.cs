namespace AdminService.Src.Application.DTOs.Get;

public class PaginatedResultDto<T>
{
    public int Page { get; set; }

    public int Limit { get; set; }

    public int TotalItems { get; set; }

    public List<T> Items { get; set; } = [];
}
