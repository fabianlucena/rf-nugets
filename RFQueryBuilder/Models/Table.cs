namespace RFQueryBuilder.Models;

public class Table
{
    public Type? Entity { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
}
