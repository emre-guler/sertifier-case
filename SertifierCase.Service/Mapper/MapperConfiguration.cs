using Mapster;
using SertifierCase.Data.Entity;
using SertifierCase.Infrastructure.Models;
using SertifierCase.Services.Models;

namespace SertifierCase.Services.Mapper;

public static class MapperConfiguration
{
    public static TypeAdapterConfig Generate()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<CreateCourse, Course>();
        config.NewConfig<Course, CourseListItem>();

        config.NewConfig<CreateAttendee, Attendee>();

        config.Compile();

        return config;
    }
}