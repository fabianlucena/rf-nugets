using Newtonsoft.Json;

namespace RFDapper
{
    public class Data
    {
        [JsonConverter(typeof(DictionarySqlGeographyConverter))]
        public Dictionary<string, object?> Values { get; set; } = [];
    }
}
