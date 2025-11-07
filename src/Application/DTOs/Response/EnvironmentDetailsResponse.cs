using System;

namespace AdminService.src.Application.DTOs.Response;

public class EnvironmentDetailsResponse
{
    public string Title { get; set; } = string.Empty;

    public Guid OwnerId { get; set; }
}
