using RFBase.Exceptions;

namespace RFQueryBuilder.Exceptions;

public class NoEntityPropertyFoundForColumnException(string column, string table)
    : HttpException(500, "No entity property found for column {0} in table {1}.", column, table)
{
}
