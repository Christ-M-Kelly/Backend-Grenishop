using System.Text.Json.Serialization;

namespace BackendGrenishop.DTOs.Response;

public class PagedResultDto<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("hasNext")]
    public bool HasNext => CurrentPage < TotalPages;

    [JsonPropertyName("hasPrevious")]
    public bool HasPrevious => CurrentPage > 1;
}
