using RFDapper.Exceptions;
using RFEntities.Entities;
using RFQueryBuilder.Implementations;
using System.Text.RegularExpressions;

namespace RFDapperMySQL;

public partial class MySQLQueryBuilder<T> : QueryBuilder<T>
    where T : Base, new()
{
    public string SchemeSeparator { get; } = "__";

    [GeneratedRegex(@"^`.*`$")]
    private static partial Regex QuotedSingleConstructor();
    private readonly static Regex QuotedSingle = QuotedSingleConstructor();

    [GeneratedRegex(@"^`.*`\.`.*`$")]
    private static partial Regex QuotedDoubleConstructor();
    private readonly static Regex QuotedDouble = QuotedDoubleConstructor();

    [GeneratedRegex(@"^`.*`\.[\w][\w\d]*$")]
    private static partial Regex QuotedAndFreeConstructor();
    private readonly static Regex QuotedAndFree = QuotedAndFreeConstructor();

    [GeneratedRegex(@"^[\w][\w\d]\.`.*`*$")]
    private static partial Regex FreeAndQuotedConstructor();
    private readonly static Regex FreeAndQuoted = FreeAndQuotedConstructor();

    public string SanitizeName(string name)
    {
        name = name.Trim();
        if (QuotedSingle.IsMatch(name))
            return name;

        if (name.Contains('`'))
            throw new InvalidTableNameException(name);

        return $"`{name}`";
    }

    public override string SanitizeTable(string table)
        => SanitizeName(table);

    public override string SanitizeColumnAlias(string alias)
        => SanitizeName(alias);

    public override string SanitizeColumn(string column)
        => SanitizeName(column);
}
