using MapsterMapper;
using SertifierCase.Data.Context;
using SertifierCase.Data.Entity;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using SertifierCase.Infrastructure.Errors;
using SertifierCase.Services.CourseService;
using SertifierCase.Services.CredentialService;
using RestSharp;
using SertifierCase.Services.SertifierIntegrationService.Enums;
using SertifierCase.Services.SertifierIntegrationService.Models;
using Microsoft.Extensions.Configuration;

namespace SertifierCase.Services.SertifierIntegrationService;

public class SertifierIntegrationService : ISertifierIntegrationService
{
    private readonly string sertifierServiceUrl;
    private readonly string sertifierSecretKey;
    private readonly SertifierCaseContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ICourseService _courseService;
    private readonly ICredentialService _credentialService;
    public SertifierIntegrationService(SertifierCaseContext dbContext,
        IMapper mapper,
        ICourseService courseService,
        ICredentialService credentialService)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var config = builder.Build();
        sertifierServiceUrl = config["serviceUrl"] ?? "";
        sertifierSecretKey = config["secretKey"] ?? "";

        _dbContext = dbContext;
        _mapper = mapper;
        _courseService = courseService;
        _credentialService = credentialService;
    }

    private async Task<RestResponse<T>> CreateRequest<T>(string jsonBody, SertifierAction action) where T : new()
    {
        string requestURL = action switch
        {
            SertifierAction.AddDetail => sertifierServiceUrl + "Detail/AddDetail",
            SertifierAction.AddDelivery => sertifierServiceUrl + "Delivery/AddDelivery",
            SertifierAction.AddRecipients => sertifierServiceUrl + "Delivery/AddRecipients",

            _ => ""
        };
        RestClient client = new RestClient($"{requestURL}");
        RestRequest request = new RestRequest();
        request.Method = action switch
        {
            SertifierAction.AddDetail => Method.Post,
            SertifierAction.AddDelivery => Method.Post,
            SertifierAction.AddRecipients => Method.Post,

            _ => Method.Post
        };
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");
        request.AddHeader("api-version", "2.3");
        request.AddHeader("secretKey", $"{sertifierSecretKey}");

        request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

        RestResponse<T> response = (RestResponse<T>)await client.ExecuteAsync<T>(request);

        return response;
    }

    public async Task DetailAndDeliveryIntegration(Course course)
    {
        Guid detailId = await CreateDetail();
        if (detailId == default(Guid)) throw new SertifierException(CustomErrors.E_100);

        Guid deliveryId = await CreateDelivery(detailId);
        if (deliveryId == default(Guid)) throw new SertifierException(CustomErrors.E_100);

        await _courseService.UpdateDeliveryId(course, deliveryId);
    }

    public async Task<Guid> CreateDetail()
    {
        CreateDetailRequestModel requestModel = new()
        {
            Type = SertifierDetailType.FieldEvent,
            Title = "string",
            Description = "string",
            Duration = SertifierDetailDurationType.Week,
            Cost = SertifierCost.Paid,
            Level = SertifierDetailLevel.Intermediate,
            Skills = new List<CreateDetailSkill> { new  CreateDetailSkill {
                Title = "string",
                SkillId = "",
                LanguageCode = "tr"
            }}
        };
        RestResponse<CreateDetailResponseModel> response = await this.CreateRequest<CreateDetailResponseModel>(JsonConvert.SerializeObject(requestModel), SertifierAction.AddDetail);
        if (response.Data != null)
            return response.Data.HasError ? default(Guid) : response.Data.Data.Id;
        return (default(Guid));
    }

    public async Task<Guid> CreateDelivery(Guid detailId)
    {
        CreateDeliveryRequestModel requestModel = new()
        {
            Title = "string",
            MailSubject = "string",
            MailBody = "string",
            EmailFromName = "string",
            EmailTemplateId = new Guid("08daef36-826f-1e5c-a95d-6ef7133a9b56"),
            DesignId = new Guid("08daef32-329c-ce21-6926-f872076528ab"),
            BadgeId = new Guid("08daef32-52e4-aa1c-706c-ddbcfdd7c55b"),
            DetailId = detailId
        };
        RestResponse<CreateDeliveryResponseModel> response = await this.CreateRequest<CreateDeliveryResponseModel>(JsonConvert.SerializeObject(requestModel), SertifierAction.AddDelivery);
        if (response.Data != null)
            return response.Data.HasError ? default(Guid) : response.Data.Data;
        return default(Guid);
    }

    public async Task RecipientIntegration()
    {
        List<CourseAttendee> courseAttendees = await _courseService.GetCompletedCourseAttendees();
        foreach (CourseAttendee courseAttendee in courseAttendees)
        {
            Credential? credential = await _credentialService.GetCredential(courseAttendee.AttendeeId, courseAttendee.CourseId);
            if (credential is not null) continue;
            RecipientRequestModel requestModel = new()
            {
                Recipients = new List<Recipient>() { new Recipient
                {
                    DeliveryId = courseAttendee.Course.DeliveryId,
                    Name = "string",
                    Email = "emre_guler01@hotmail.com",
                    IssueDate = DateTime.UtcNow.Date,
                    QuickPublish = true,
                    ExternalId = courseAttendee.AttendeeId.ToString(),
                    Attributes = new List<AddRecipientAttribute> {}
                }}
            };
            RestResponse<AddRecipientsResponseModel> response = await this.CreateRequest<AddRecipientsResponseModel>(JsonConvert.SerializeObject(requestModel), SertifierAction.AddRecipients);
            if (response.Data != null && !response.Data.HasError)
            {
                AddRecipientsData recipient = JsonConvert.DeserializeObject<AddRecipientsData>(response.Data.Data.ToString());
                foreach (var rec in recipient.Data)
                {
                    foreach (AddRecipientsDataDetail recDetail in rec.Value)
                    {
                        Course? courseData = await _courseService.GetCourseByDeliveryId(new Guid(rec.Key.ToString()));
                        if (courseData is not null)
                            await _credentialService.Create(new Credential
                            {
                                CreateDate = DateTime.UtcNow,
                                UpdateDate = DateTime.UtcNow,
                                CredentialNo = recDetail.CertificateNo,
                                CourseId = courseData.Id,
                                AttendeeId = recDetail.ExternalId
                            });
                    }
                }
            }
        }
    }
}