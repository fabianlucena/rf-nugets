using RFBase.Exceptions;

namespace RFQueryBuilder.Exceptions;

public class TableIsNotSetException() : HttpException(500, "Table is not set.")
{
}
