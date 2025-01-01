using Newtonsoft.Json;
using SertifierCase.Services.SertifierIntegrationService.Enums;

namespace SertifierCase.Services.SertifierIntegrationService.Models;
public class CreateDetailRequestModel
{
    [JsonProperty("type")]
    public SertifierDetailType Type { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }
    [JsonProperty("durationType")]
    public SertifierDetailDurationType Duration { get; set; }
    [JsonProperty("cost")]
    public SertifierCost Cost { get; set; }
    [JsonProperty("level")]
    public SertifierDetailLevel Level { get; set; }
    [JsonProperty("skills")]
    public List<CreateDetailSkill> Skills { get; set; }
}

public class CreateDetailSkill
{
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("skillId")]
    public string SkillId { get; set; }
    [JsonProperty("languageCode")]
    public string LanguageCode { get; set; }
}


