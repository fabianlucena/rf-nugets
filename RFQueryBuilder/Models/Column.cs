namespace RFQueryBuilder.Models;

public class Column(string query = "", string alias = "")
{
    public virtual string Query { get; set; } = query;
    public virtual string Alias { get; set; } = alias;
}
