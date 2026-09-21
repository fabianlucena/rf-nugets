using RFBase.Exceptions;

namespace RFQueryBuilder.Exceptions;

public class ColumnDoesNotExistInTableException(string column, string table)
    : HttpException(500, "Column {0} does not exist in table {1}.", column, table)
{
}
