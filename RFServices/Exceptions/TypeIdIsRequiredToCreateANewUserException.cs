using RFBase.Exceptions;

namespace RFServices.Exceptions;

public class TypeIdIsRequiredToCreateANewUserException()
    : HttpException(400, "TypeId is required to create a new user")
{ }
