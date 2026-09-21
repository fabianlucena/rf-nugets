using RFBase.Exceptions;

namespace RFQueryBuilder.Exceptions;

public class TableNameIsNotSetException() : HttpException(500, "Table name is not set.")
{
}
