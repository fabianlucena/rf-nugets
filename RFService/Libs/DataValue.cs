using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text.Json;

namespace RFService.Libs
{
    public static class DataValue
    {
        public static object? GetPropertyValue(dynamic obj, string prop)
        {
            return GetValue(obj[prop]);
        }

        public static bool TryGetPropertyValue(dynamic obj, string prop, out dynamic? value)
        {
            if (!obj.TryGetValue(prop, out value))
                return false;

            try
            {
                value = GetValue(value);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static object? GetValue(object? element, bool camelize = false)
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

                case JsonValueKind.Array:
                    {
                        List<object?> result = [];
                        foreach (var item in jsonElement.EnumerateArray())
                            result.Add(GetValue(item));

                        return result;
                    }

                case JsonValueKind.Object:
                    {
                        Dictionary<string, object?> result = [];
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

        public static Dictionary<string, object?> PascalizeDictionary(IDictionary<string, object?> data)
        {
            var result = new Dictionary<string, object?>();

            foreach (var item in data)
            {
                var name = item.Key;
                if (Char.IsUpper(name[0]))
                {
                    result[name] = item.Value;
                    continue;
                }

                var Name = char.ToUpper(name[0], CultureInfo.InvariantCulture) + name[1..];
                if (data.ContainsKey(Name))
                    continue;

                result[Name] = item.Value;
            }

            return result;
        }

        public static T ObjectFromDictionary<T>(Dictionary<string, object?> data)
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
                if (!data.TryGetValue(name, out object? value))
                    continue;

                property.SetValue(obj, GetValue(value));
            }

            return obj;
        }

        public static Dictionary<string, object?> GetPascalizeQueryDictionaryFromHttpContext(HttpContext httpContext)
        {
            return httpContext.Request.Query.ToDictionary(
                k => char.ToUpper(k.Key[0], CultureInfo.InvariantCulture) + k.Key[1..],
                v => {
                    var value = v.Value;
                    if (value.Count == 0)
                        return "";

                    string json;
                    if (value.Count > 1)
                        json = $"[${string.Join(',', value.Select(v => v ?? ""))}]";
                    else if (value[0] == null)
                        return "";
                    else
                        json = value[0] ?? "";

                    return GetValue(JsonSerializer.Deserialize<object?>(json), true);
                }
            );
        }

        public static bool TryGetValue<T>(IDictionary<string, object?> data, string name, out T? value)
            where T : struct
        {
            if (data.TryGetValue(name, out var obj)
                && obj is T val
            )
            {
                value = val;
                return true;
            }

            value = default;
            return true;
        }
    }
}
