using RFBase.Exceptions;

namespace RFOauth2Client.Exceptions;

public class UnsupportedContentTypeException(string endpoint, string contentType)
    : HttpException(400, "Unsupported content type for endpoint {endpoint}: {contentType}", endpoint, contentType)
{
}
