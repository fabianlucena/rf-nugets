using RFOperators;

namespace RFService.Repo
{
    public class FromList
        : List<From>
    {
        public FromList() { }
        public FromList(FromList? fromList)
            : base(fromList ?? [])
        {
            if (fromList != null)
                AddRange(fromList);
        }

        public void Add(string propertyName, string? alias = null)
            => Add(new From(propertyName: propertyName, alias: alias));
    }
}
