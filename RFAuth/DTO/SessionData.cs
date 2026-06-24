using System.Dynamic;

namespace RFAuth.DTO;

public class SessionData() : DynamicObject
{
    private readonly Dictionary<string, object?> data = [];

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        return data.TryGetValue(binder.Name, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        data[binder.Name] = value;
        return true;
    }

    public object? this[string key]
    {
        get => data.TryGetValue(key, out var v) ? v : null;
        set => data[key] = value;
    }

    public IEnumerable<KeyValuePair<string, object?>> GetAll()
        => data;

    public SessionData Clone()
    {
        var clone = new SessionData();
        foreach (var kvp in data)
            clone[kvp.Key] = kvp.Value;

        return clone;
    }

    public Dictionary<string, object?> ToDictionary()
        => new(data);
}
