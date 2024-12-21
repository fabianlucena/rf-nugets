using RFService.Repo;

namespace RFDapper
{
    public interface IDriver
    {
        string GetDefaultSchema();

        string SanitizeSchema(string schema);
        
        string SanitizeVarname(string varname);

        string GetFullColumnName(string key, GetOptions options, string? defaultAlias);
    }
}
