

using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs;
public class ApiResponse<T>
{
    [JsonProperty("success")]
    public bool Success { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
    [JsonProperty("data")]
    public T? Data { get; set; }
    public object? Metadata { get; set; }

    public ApiResponse(bool success, string message, T? data, object? metadata = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Metadata = metadata;
    }
}
