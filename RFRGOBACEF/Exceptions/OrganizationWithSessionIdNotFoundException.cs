using RFBase.Exceptions;

namespace RFRGOBACEF.Exceptions;

public class OrganizationWithSessionIdNotFoundException(long sessionId)
    : HttpException(400, "Organization with SessionId {sessionId} not found.", sessionId.ToString())
{ }
