namespace RFQueryBuilder.Models;

public class EntityColumn(string name, Type type, string query, string alias, bool isMapeable, bool isPrimaryKey) : Column()
{
    public string Name { get; } = name;
    public Type Type { get; } = type;
    public new string Query { get; } = query;
    public new string Alias { get; } = alias;
    public bool IsMapeable { get; } = isMapeable;
    public bool IsPrimaryKey { get; } = isPrimaryKey;
}
