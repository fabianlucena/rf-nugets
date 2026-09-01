using RFBase.Exceptions;

namespace RFRGOBAC.Exceptions;

public class ThereAreMultipleUsersMatchingTheGivenConditionsException()
    : HttpException(400, "There are multiple users matching the given conditions.")
{
}
