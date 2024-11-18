using Newtonsoft.Json;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;
namespace RFDapper
{
    public class DictionarySqlGeographyConverter
        : JsonConverter<Dictionary<string, object?>>
    {
        public override void WriteJson(
            JsonWriter writer,
            Dictionary<string, object?>? value,
            JsonSerializer serializer
        )
        {
            var jobject = new JObject();

            if (value != null)
            {
                foreach (var kvp in value)
                {
                    if (kvp.Value is SqlGeography geography)
                    {
                        jobject[kvp.Key] = new string(geography.STAsText().Value);
                    }
                    else if (kvp.Value == null)
                    {
                        jobject[kvp.Key] = null;
                    }
                    else
                    {
                        jobject[kvp.Key] = JToken.FromObject(kvp.Value, serializer);
                    }
                }
            }

            jobject.WriteTo(writer);
        }
        public override Dictionary<string, object?> ReadJson(
            JsonReader reader,
            Type objectType,
            Dictionary<string, object?>? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            var dictionary = new Dictionary<string, object?>();
            var jobject = JObject.Load(reader);
            
            foreach (var property in jobject.Properties())
            {
                if (property.Value.Type == JTokenType.String)
                {
                    string value = property.Value.ToString();
                    if (IsWktFormat(value))
                    {
                        dictionary[property.Name] = SqlGeography.STGeomFromText(new System.Data.SqlTypes.SqlChars(value), 4326);
                    }
                    else
                    {
                        dictionary[property.Name] = value;
                    }
                }
                else
                {
                    dictionary[property.Name] = property.Value.ToObject<object>(serializer);
                }
            }
            
            return dictionary;
        }
        
        private static bool IsWktFormat(string value)
        {
            // Verifica si el valor es un WKT (Well-Known Text) válido para SqlGeography
            return value.StartsWith("POINT(") || value.StartsWith("LINESTRING(") || value.StartsWith("POLYGON("); }
        }
}
