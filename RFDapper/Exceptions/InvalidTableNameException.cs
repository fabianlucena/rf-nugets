using RFBase.Exceptions;

namespace RFDapper.Exceptions;

[Serializable]
public class InvalidTableNameException(string table)
    : HttpException(500, "Invalid table name: {0}", table)
{
}