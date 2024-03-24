using System.Text.Json.Serialization;

namespace SendEmail.API.DTOs;

public class EmailDto
{
    [JsonPropertyName("Nome")]
    public string Nome { get; set; }

    [JsonPropertyName("Email")]
    public string Email { get; set; }

    [JsonPropertyName("Message")]
    public string Message { get; set; }
}