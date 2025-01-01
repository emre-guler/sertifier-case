using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using Hangfire;
using Hangfire.MySql;
using SertifierCase.Data.Context;
using SertifierCase.Infrastructure.Middlewares;
using SertifierCase.Services.CourseService;
using SertifierCase.Services.AttendeeService;
using SertifierCase.Services.CredentialService;
using SertifierCase.Services.Mapper;
using SertifierCase.Services.SertifierIntegrationService;
using SertifierCase.API.RecurringJobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MYSQL_CONNECTION");
builder.Services.AddDbContext<SertifierCaseContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

var config = MapperConfiguration.Generate();

builder.Services.AddSingleton(config);
builder.Services.AddSingleton<IMapper, ServiceMapper>();

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IAttendeeService, AttendeeService>();
builder.Services.AddScoped<ICredentialService, CredentialService>();

builder.Services.AddScoped<ISertifierIntegrationService, SertifierIntegrationService>();

builder.Services.AddHangfire(x => x.UseStorage(
    new MySqlStorage(
        connectionString,
        new MySqlStorageOptions
        {
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 50000,
            TransactionTimeout = TimeSpan.FromMinutes(1)
        }
    )
));
builder.Services.AddHangfireServer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHangfireServer(new BackgroundJobServerOptions());
Jobs.ScheduledTask();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
