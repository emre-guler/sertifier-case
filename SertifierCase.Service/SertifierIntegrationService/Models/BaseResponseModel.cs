namespace SertifierCase.Services.SertifierIntegrationService.Models;
public abstract class BaseResponseModel
{
    public string Message { get; set; }
    public bool HasError { get; set; }
    public List<string>? ValidationErrors { get; set; }
}