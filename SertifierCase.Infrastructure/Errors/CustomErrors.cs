using SertifierCase.Infrastructure.Models;

namespace SertifierCase.Infrastructure.Errors;

public static class CustomErrors
{
    public static Response E_100 = new(true, "Something went wrong!", null, null);
    public static Response E_101 = new(true, "Must be filled!", null, null);
    public static Response E_102 = new(true, "Already exist!", null, null);
    public static Response E_103 = new(true, "Attendee not found!", null, null);
    public static Response E_104 = new(true, "Course not found!", null, null);
    public static Response E_105 = new(true, "Already enrolled", null, null);
    public static Response E_106 = new(true, "", null, null);
}