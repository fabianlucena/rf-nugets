using RFBase.Exceptions;

namespace RFRBAC.Exceptions;

public class SomeRolesDoNotExistException(IEnumerable<string> inexistentRoles)
    : HttpException(500, "Some roles do not exist: {0}", string.Join(", ", inexistentRoles))
{}