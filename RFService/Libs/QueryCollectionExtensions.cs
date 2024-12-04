using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace RFService.Libs
{
    public static class QueryCollectionExtensions
    {
        public static QueryCollection GetPascalized(this IQueryCollection data)
        {
            var result = new Dictionary<string, StringValues>();

            foreach (var item in data)
            {
                var name = item.Key;
                if (Char.IsUpper(name[0]))
                {
                    result[name] = item.Value;
                    continue;
                }

                var Name = char.ToUpper(name[0], CultureInfo.InvariantCulture) + name[1..];
                if (result.ContainsKey(Name))
                    continue;

                result[Name] = item.Value;
            }

            return new QueryCollection(result);
        }

        static public List<string> GetStrings(this IQueryCollection query, string name)
        {
            if (!query.TryGetValue(name, out StringValues value))
                return [];

            return value.Select(i => i ?? "").ToList();
        }

        static public string GetString(this IQueryCollection query, string name)
        {
            if (!query.TryGetValue(name, out StringValues value))
                return "";

            return string.Join(",", value.Where(val => val != null));
        }

        static public bool TryGetGuid(this IQueryCollection query, string name, out Guid value)
        {
            if (!query.TryGetValue(name, out StringValues strings)
                || strings.Count != 1
                || string.IsNullOrEmpty(strings[0])
            )
            {
                value = default;
                return false;
            }

            value = Guid.Parse(strings[0] ?? "");
            return value != Guid.Empty;

        }

        static public Guid GetGuid(this IQueryCollection query, string name)
            => Guid.Parse(GetString(query, name));

        static public bool TryGetBool(this IQueryCollection query, string name, out bool value)
        {
            if (!query.TryGetValue(name, out StringValues strings)
                || strings.Count != 1
                || strings[0] is null
            )
            {
                value = false;
                return false;
            }

            var str = strings[0];
            value = !(
                string.IsNullOrWhiteSpace(str) ||
                str.Trim().Equals("FALSE", StringComparison.CurrentCultureIgnoreCase)
            );

            return true;
        }

        static public bool GetBool(this IQueryCollection query, string name)
        {
            if (!query.TryGetValue(name, out StringValues strings)
                || strings.Count == 0
            )
                return false;

            return !strings.Any(i =>
                string.IsNullOrWhiteSpace(i) ||
                i.Trim().Equals("FALSE", StringComparison.CurrentCultureIgnoreCase) ||
                i.Trim() == ""
            );
        }
    }
}
