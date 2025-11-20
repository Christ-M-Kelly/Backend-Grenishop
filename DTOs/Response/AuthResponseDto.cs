using System.Text.Json.Serialization;

namespace BackendGrenishop.DTOs.Response;

public class AuthResponseDto
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("user")]
    public UserProfileDto? User { get; set; }

    [JsonPropertyName("errors")]
    public List<string>? Errors { get; set; }
}
