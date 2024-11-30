namespace RFService.Repo
{
    public class From
    {
        public string? Alias { get; set; }

        public Dictionary<string, From> Join { get; set; } = [];

        public From()
            {}

        public From(From src)
        {
            Alias = src.Alias;
            foreach (var join in src.Join)
                Join[join.Key] = new From(join.Value);
        }

        public From(string? alias)
        {
            Alias = alias;
        }

        public List<string> GetAllAlias() {
            List<string> allAlias = [];

            if (!string.IsNullOrWhiteSpace(Alias))
                allAlias.Add(Alias);

            foreach (var join in Join)
                allAlias.AddRange(join.Value.GetAllAlias());

            return allAlias;
        }

        public bool ExistsAlias(string alias)
            => GetAllAlias().Any(i => i == alias);

        public string CreateAlias(string? alias)
        {
            var allAlias = GetAllAlias();
            if (string.IsNullOrWhiteSpace(Alias))
                alias = "t";

            if (!allAlias.Any(i => i == alias))
                return alias ?? "t";

            var counter = 0;
            var newAlias = Alias + counter;
            if (!allAlias.Any(i => i == alias))
                return newAlias;

            do
            {
                counter++;
                newAlias = Alias + counter;
            } while (allAlias.Any(i => i == newAlias));

            return newAlias;
        }

        public string GetOrCreateAlias(string? alias)
        {
            if (!string.IsNullOrWhiteSpace(Alias))
                return Alias;

            return CreateAlias(alias);
        }
    }
}
