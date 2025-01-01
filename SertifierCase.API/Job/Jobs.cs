using Hangfire;
using SertifierCase.Services.SertifierIntegrationService;

namespace SertifierCase.API.RecurringJobs;

public static class Jobs
{
    [Obsolete]
    public static void ScheduledTask()
    {
        RecurringJob.RemoveIfExists(nameof(ISertifierIntegrationService.RecipientIntegration));
        RecurringJob.AddOrUpdate<ISertifierIntegrationService>(x => x.RecipientIntegration(), Cron.MinuteInterval(5));
    }
}