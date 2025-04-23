using System.Net;

namespace RO.DevTest.Domain.Exception;

/// <summary>
/// Returns a <see cref="HttpStatusCode.NotFound"/> to the request
/// </summary>
/// <remarks>
/// Initializes a new instance of <see cref="NotFoundException"/>
/// </remarks>
/// <param name="name">The name of the resource</param>
/// <param name="key">The identifier of the resource</param>
public class NotFoundException(string name, object key) : ApiException($"{name} with id ({key}) was not found.")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
}