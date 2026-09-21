using Dapper;
using RFBase.Libs;

namespace RFDapper.Extensions;

public static class DataDictionaryExtensions
{
    public static DynamicParameters ToDynamicParameters(this DataDictionary dataDictionary)
    {
        var parameters = new DynamicParameters();
        foreach (var kv in dataDictionary)
            parameters.Add(kv.Key, kv.Value);

        return parameters;
    }
}
