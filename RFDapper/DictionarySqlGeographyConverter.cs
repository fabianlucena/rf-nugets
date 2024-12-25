using Newtonsoft.Json;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;
using RFService.Libs;
namespace RFDapper
{
    public class DictionarySqlGeographyConverter
        : JsonConverter<DataDictionary>
    {
        public override void WriteJson(
            JsonWriter writer,
            DataDictionary? value,
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
        public override DataDictionary ReadJson(
            JsonReader reader,
            Type objectType,
            DataDictionary? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
        )
        {
            var dictionary = new DataDictionary();
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
