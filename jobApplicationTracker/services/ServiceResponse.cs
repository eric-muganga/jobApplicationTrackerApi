namespace jobApplicationTrackerApi.services;

/// <summary>
/// Generic wrapper for web api response.       
/// </summary>
/// <typeparam name="T"></typeparam>
public class ServiceResponse<T>
{
    public T Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = null;
    public List<string> ErrorMessages { get; set; } = null;
    public Enum StatusCode { get; set; }
}