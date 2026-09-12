using RFBase.Exceptions;

namespace RFServices.Exceptions;

public class SystemUserNotFoundException()
    : HttpException(500, "System user not found. Please ensure that a user with the username 'system' exists.")
{ 
}
