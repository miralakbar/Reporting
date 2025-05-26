using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.DTOs.Base;

public class PageQueryDto
{
    [DefaultValue("1")]
    [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; } = 1;

    [DefaultValue("10")]
    [Range(1, 500, ErrorMessage = "The value must be between 1 and 500")]
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 100;
}
