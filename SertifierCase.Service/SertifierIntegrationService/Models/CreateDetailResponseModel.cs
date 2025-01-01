namespace SertifierCase.Services.SertifierIntegrationService.Models;
public class CreateDetailResponseModel : BaseResponseModel
{
    public CreateDetailData Data { get; set; }
}

public class CreateDetailData
{
    public Guid Id { get; set; }
}