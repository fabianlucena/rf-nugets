using System.Text.Json;

namespace RFService.Services
{
    public static class DataValue
    {
        public static dynamic? GetPropertyValue(dynamic obj, string prop)
        {
            var value = obj[prop];
            if (value.GetType() == typeof(JsonElement))
                return GetValue(value);

            return value;
        }

        public static dynamic? GetValue(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String: return element.GetString();
                case JsonValueKind.Array:
                    {
                        List<object?> result = [];
                        foreach (var item in element.EnumerateArray())
                            result.Add(GetValue(item));

                        return result;
                    }
            }

            throw new Exception("Valor no implementado");
        }
    }
}
