using SertifierCase.Infrastructure.Models;

namespace SertifierCase.Infrastructure.Errors;

public class SertifierException : Exception
{
    public SertifierException(Response response) : base(response.message)
    {
        Response = response;
    }
    public Response Response { get; set; }
}