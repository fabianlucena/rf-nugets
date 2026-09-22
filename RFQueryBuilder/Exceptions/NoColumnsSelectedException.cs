using RFBase.Exceptions;

namespace RFQueryBuilder.Exceptions;

public class NoColumnsSelectedException(string table)
    : HttpException(500, "No columns selected in table {0}.", table)
{
}
