using Newtonsoft.Json;
using RFService.Libs;

namespace RFDapper
{
    public class Data
    {
        [JsonConverter(typeof(DictionarySqlGeographyConverter))]
        public DataDictionary Values { get; set; } = [];
    }
}
