using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Domain.DTOs.Base;

public class PagedResult<T>
{
    [JsonPropertyName("pageSize")]
    public int? PageSize { get; set; }

    [JsonPropertyName("pageNumber")]
    public int? PageNumber { get; set; }

    [JsonPropertyName("totalPages")]
    public int? TotalPages => CalculateTotalPages();

    [JsonPropertyName("totalCount")]
    public int? TotalCount { get; set; }

    [JsonPropertyName("payload")]
    public List<T> Payload { get; set; }

    private int? CalculateTotalPages()
    {
        if (PageSize.HasValue && PageSize.Value > 0 && TotalCount.HasValue)
        {
            return (int)Math.Ceiling((double)TotalCount.Value / PageSize.Value);
        }

        return 0;
    }
}