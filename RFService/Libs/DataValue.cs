using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace RFService.Libs
{
    public static class DataValue
    {
        public static dynamic? GetPropertyValue(dynamic obj, string prop)
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

        public static dynamic? GetValue(object? element)
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
                    List<object?> result = [];
                    foreach (var item in jsonElement.EnumerateArray())
                        result.Add(GetValue(item));

                    return result;
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

        public static Dictionary<string, string> GetPascalizeQueryDictionaryFromHttpContext(HttpContext httpContext)
            => httpContext.Request.Query.ToDictionary(
                k => char.ToUpper(k.Key[0], CultureInfo.InvariantCulture) + k.Key[1..],
                v => v.Value.ToString()
            );
    }
}
