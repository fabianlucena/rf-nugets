using RFOperators;

namespace RFService.Repo
{
    public class From
    {
        public JoinType? Type { get; set; } = null;

        public string? PropertyName { get; set; }

        public Type? Entity { get; set; }

        public string? Alias { get; set; }

        public Operator? On { get; set; }

        public List<From> Join { get; set; } = [];

        public From(From? from)
        {
            if (from != null)
            {
                Type = from.Type;
                PropertyName = from.PropertyName;
                Entity = from.Entity;
                Alias = from.Alias;
                On = from.On;
                Join = from.Join;
            }
        }

        public From(
            Type entity,
            string? propertyName = null
        )
        {
            Type = null;
            PropertyName = propertyName;
            Entity = entity;
            Alias = null;
            On = null;
        }

        public From(
            string? propertyName = null,
            string? alias = null,
            JoinType? type = null,
            Type? entity = null,
            Operator? on = null
        )
        {
            Type = type;
            PropertyName = propertyName;
            Entity = entity;
            Alias = alias;
            On = on;
        }

        public List<string> GetAllAlias() {
            List<string> allAlias = [];

            if (!string.IsNullOrWhiteSpace(Alias))
                allAlias.Add(Alias);

            foreach (var join in Join)
                allAlias.AddRange(join.GetAllAlias());

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
