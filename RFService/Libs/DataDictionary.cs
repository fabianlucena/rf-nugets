using RFService.ILibs;
using System.Collections;
using System.Globalization;
using System.Text.Json;

namespace RFService.Libs
{
    public class DataDictionary
        : Dictionary<string, object?>,
            IDataDictionary
    {
        public DataDictionary()
        { }
        
        public DataDictionary(IDictionary<string, object?>? data)
            : base(data ?? new Dictionary<string, object?>())
        { }

        public DataDictionary GetPascalized()
        {
            var data = this.ToDictionary(
                i => char.ToUpper(i.Key[0], CultureInfo.InvariantCulture) + i.Key[1..],
                i => {
                    var value = i.Value;
                    if (value != null
                        && value.GetType() == typeof(DataDictionary)
                    )
                        value = ((DataDictionary)value).GetPascalized();
                
                    return GetValue(value);
                }
            );

            return new DataDictionary(data);
        }

        public bool IsNullValue(string key)
        {
            return TryGetValue(key, out object? obj)
                || obj is null;
        }

        public bool IsNotNullValue(string key)
        {
            return TryGetValue(key, out object? obj)
                || obj is not null;
        }

        public bool TryGetInt64(string key, out Int64 value)
        {
            if (!TryGetValue(key, out object? obj)
                || obj is null
                || obj is not Int64 val
            )
            {
                value = 0;
                return false;
            }

            value = val;
            return true;
        }

        public string? GetString(string key)
        {
            var obj = GetValue(this[key]);
            if (obj is null
                || obj is not string str
            )
            {
                return null;
            }

            return str;
        }

        public bool TryGetNotNullString(string key, out string value)
        {
            if (!TryGetValue(key, out object? obj)
                || obj is null
            )
            {
                value = "";
                return false;
            }

            var val = GetValue(obj);
            if (val is null
                || val is not string str
            )
            {
                value = "";
                return false;
            }

            value = str;
            return true;
        }

        public bool TryGetNotNullOrEmptyString(string key, out string value)
        {
            if (!TryGetValue(key, out object? obj)
                || obj is null
            )
            {
                value = "";
                return false;
            }

            var val = GetValue(obj);
            if (val is null
                || val is not string str
            )
            {
                value = "";
                return false;
            }

            value = str;
            return !string.IsNullOrEmpty(value);
        }

        public bool TryGetNotNullStrings(string key, out IEnumerable<string> value)
        {
            if (!TryGetValue(key, out object? obj)
                || obj is null
            )
            {
                value = [];
                return false;
            }

            if (obj is string[] valueStrings)
            {
                value = valueStrings;
                return true;
            }

            if (obj is IEnumerable<string> strings)
            {
                value = strings;
                return true;
            }

            if (obj is not IEnumerable list)
            {
                value = [];
                return false;
            }

            var newValue = new List<string>();
            foreach (var item in list)
            {
                if (item == null)
                    continue;

                var str = item.ToString();
                if (string.IsNullOrEmpty(str))
                    continue;

                newValue.Add(str);
            }

            value = newValue;
            return true;
        }

        public bool TryGetBool(string key, out bool value)
        {
            if (!TryGetValue(key, out object? obj)
                || obj is null
            )
            {
                value = default;
                return false;
            }

            if (obj is bool val)
            {
                value = val;
                return true;
            }

            obj = GetValue(obj);
            if (obj is bool val1)
            {
                value = val1;
                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetGuid(string key, out Guid value)
        {
            if (!TryGetValue(key, out object? obj)
                || obj is null
            )
            {
                value = default;
                return false;
            }

            if (obj is Guid guid)
            {
                value = guid;
                return true;
            }

            var str = obj.ToString();
            if (string.IsNullOrEmpty(str))
            {
                value = default;
                return false;
            }

            guid = Guid.Parse(str);
            if (guid == Guid.Empty)
            {
                value = default;
                return false;
            }

            value = guid;
            return true;
        }

        public bool TryGetGuids(string key, out IEnumerable<Guid> value)
        {
            if (!TryGetValue(key, out object? obj)
                || obj is null
            )
            {
                value = [];
                return false;
            }

            obj = GetValue(obj);

            if (obj is Guid[] guids)
            {
                value = guids;
                return true;
            }

            if (obj is IEnumerable<Guid> eguids)
            {
                value = eguids;
                return true;
            }

            if (obj is not IEnumerable list)
            {
                value = [];
                return false;
            }

            var newValue = new List<Guid>();
            foreach (var item in list)
            {
                if (item == null)
                {
                    value = [];
                    return false;
                }

                if (item is Guid guid)
                {
                    newValue.Add(guid);
                    continue;
                }

                var str = item.ToString();
                if (string.IsNullOrEmpty(str))
                {
                    value = [];
                    return false;
                }

                guid = Guid.Parse(str);
                if (guid == Guid.Empty)
                {
                    value = [];
                    return false;
                }

                newValue.Add(guid);
            }

            value = newValue;
            return true;
        }

        public bool TryGetNullableObjects(string key, out IEnumerable<object?> value)
        {
            if (!TryGetValue(key, out object? obj)
                || obj is null
            )
            {
                value = [];
                return false;
            }

            if (obj is IEnumerable<object?> objects)
            {
                value = objects;
                return true;
            }

            if (obj is IEnumerable<object?> eobjects)
            {
                value = eobjects;
                return true;
            }

            if (obj is not IEnumerable list)
            {
                value = [];
                return false;
            }

            value = list.Cast<object?>();
            return true;
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (!TryGetValue(key, out object? obj)
                || obj is null
            )
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                value = default;
#pragma warning restore CS8601 // Possible null reference assignment.
                return false;
            }

            if (obj is T val)
            {
                value = val;
                return true;
            }

#pragma warning disable CS8601 // Possible null reference assignment.
            value = default;
#pragma warning restore CS8601 // Possible null reference assignment.

            return false;
        }

        public T ToObject<T>()
            where T : new()
        {
            var obj = new T();
            Type type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property == null || !property.CanWrite)
                    continue;

                var name = property.Name;
                if (property.PropertyType == typeof(string))
                {
                    if (TryGetNotNullString(name, out var value))
                        property.SetValue(obj, value);
                }
                else if (property.PropertyType == typeof(Int64))
                {
                    if (TryGetInt64(name, out var value))
                        property.SetValue(obj, value);
                }
                else if (property.PropertyType == typeof(bool))
                {
                    if (TryGetBool(name, out var value))
                        property.SetValue(obj, value);
                }
                else if (IsNullValue(name))
                    property.SetValue(obj, null);
                else if (ContainsKey(name))
                    throw new NotImplementedException($"Conversion del tipo {property.PropertyType} no implmentado en el método DataDictionary.ToObject");
            }

            return obj;
        }

        public object? GetValue(object? element, bool camelize = false)
        {
            if (element == null)
                return null;

            if (element.GetType() != typeof(JsonElement))
                return element;

            var jsonElement = (JsonElement)element;
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.String:
                    return jsonElement.GetString();

                case JsonValueKind.True:
                    return true;

                case JsonValueKind.False:
                    return false;

                case JsonValueKind.Null:
                    return null;

                case JsonValueKind.Number:
                    if (jsonElement.TryGetInt64(out var int32))
                        return int32;
                    else if (jsonElement.TryGetUInt64(out var uint32))
                        return uint32;
                    else
                        return jsonElement.GetDouble();

                case JsonValueKind.Array:
                    {
                        List<object?> result = [];
                        foreach (var item in jsonElement.EnumerateArray())
                            result.Add(GetValue(item));

                        return result;
                    }

                case JsonValueKind.Object:
                    {
                        DataDictionary result = [];
                        if (camelize)
                        {
                            foreach (var item in jsonElement.EnumerateObject())
                                result[char.ToUpper(item.Name[0], CultureInfo.InvariantCulture) + item.Name[1..]] = GetValue(item.Value);
                        }
                        else
                        {
                            foreach (var item in jsonElement.EnumerateObject())
                                result[item.Name] = GetValue(item.Value);
                        }

                        return result;
                    }
            }

            throw new Exception("Valor no implementado");
        }

        public string GetJson()
        {
            try
            {
                return JsonSerializer.Serialize(this);
            }
            catch { }

            var lines = new List<string>();
            foreach (var kv in this)
            {
                string value;
                try
                {
                    value = JsonSerializer.Serialize(kv.Value);
                }
                catch
                {
                    try
                    {
                        if (kv.Value == null)
                            value = "null";
                        else
                            value = '"' + (kv.Value?.ToString()?.Replace("\"", "\\\"") ?? "") + '"';
                    }
                    catch
                    {
                        value = "\"*** Error to convert value ***\"";
                    }
                }

                lines.Add($"\"{kv.Key.Replace("\"", "\\\"")}\":{value}");
            }

            return "{" + string.Join(",", lines) + "}";
        }
    }
}
