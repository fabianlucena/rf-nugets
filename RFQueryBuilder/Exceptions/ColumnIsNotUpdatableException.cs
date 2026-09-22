using RFBase.Exceptions;

namespace RFQueryBuilder.Exceptions;

public class ColumnIsNotUpdatableException(string column, string table)
    : HttpException(500, "Column {0} is not updatable in table {1}.", column, table)
{
}
